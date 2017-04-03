using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BCore.Dal.BlogModels;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
//using BCore.Models.ViewModels;
using BCore.Models.ViewModels.Blog;
using Backload.MiddleWare;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using BCore.Models.Commands;
using BCore.Dal;

namespace BCore
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature",
                _getFeatureName(controller.ControllerType));
        }


        private string _getFeatureName(TypeInfo controllerType)
        {
            string[] tokens = controllerType.FullName.Split('.');

            if (!tokens.Any(t => t == "Features"))
                return "";

            string featureName = tokens
                .SkipWhile(t => !t.Equals("features", StringComparison.CurrentCultureIgnoreCase))
                .Skip(1)
                .Take(1)
                .FirstOrDefault();

            return featureName;
        }
    }

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }        

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DevConnection");

            services.AddDbContext<BlogDbContext>(options => 
                options.UseSqlServer(connection));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/Login";
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<BlogDbContext>()
            .AddDefaultTokenProviders();
            
            // Add MVC services to the services container
            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(options => 
            {
                options.ViewLocationFormats.Insert(0, "/Views/Shared/Blog/{0}.cshtml");
            });
            
            // Add session related services.
            services.AddSession();          

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageStore",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageStore", "Allowed");
                    });
            });

            _configureAutoMapper(services);

            services.AddScoped<IUpdateCommands, UpdateCommands>();
            services.AddScoped<IPostCommands, PostCommands>();
            services.AddScoped<IFeedCommands, FeedCommands>();
            services.AddScoped<ITopCommands, TopCommands>();
            services.AddScoped<IUoW, Unit>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure Session.
            app.UseSession();

            // Add static files to the request pipeline           

            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                    context.Context.Response.Headers.Append("Pragma", "no-cache");
                    context.Context.Response.Headers.Append("Expires", "0");
                }
            });

            app.UseBackload();

            _seed(app);
        }

        private void _configureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.CreateMap<ICollection<Post>, UpdateViewModel>()
                    .ForMember(g => g.RecentPosts, o => o.MapFrom(c => c));

                config.CreateMap<ICollection<Post>, FeedViewModel>()
                    .ForMember(g => g.RecentPosts, o => o.MapFrom(c => c));

                config.CreateMap<ICollection<Post>, TopViewModel>()
                    .ForMember(g => g.RecentPosts, o => o.MapFrom(c => c));

                config.CreateMap<Post, PostViewModel>()
                    .ForMember(g => g.Parts, o => o.MapFrom(c => c.Parts))
                    .ForMember(g => g.Comments, o => o.MapFrom(c => c.Comments))
                    .ForMember(g => g.Hashes, o => o.MapFrom(c => c.PostHashes));
                                 
                config.CreateMap<Part, PartViewModel>();
                config.CreateMap<Comment, CommentViewModel>();
                config.CreateMap<PostHash, HashViewModel>()
                    .ForMember(g => g.Id, o => o.MapFrom(c => c.HashId))
                    .ForMember(g => g.Tag, o => o.MapFrom(c => c.Hash.Tag));
                
                config.CreateMap<WhatsNewViewModel, PartViewModel>()
                      .ForMember(g => g.Value, o => o.MapFrom(c => c.GetPartValue()))
                      .ForMember(g => g.PartType, o => o.MapFrom(c => c.GetPartTypeName()))
                      .ForMember(g => g.DateTime, o => o.MapFrom(c => DateTime.Now));

                config.CreateMap<User, UserViewModel>();

                config.CreateMap<UpdateViewModel, Post>()        
                    .ForMember(g => g.DateTime, o => o.MapFrom(c => c.PreviewPost.DateTime))            
                    .ForMember(g => g.Parts, o => o.MapFrom(c => c.PreviewPost.Parts));
                                
                config.CreateMap<PartViewModel, Part>();
                
                config.CreateMap<WhatsThinkViewModel, Comment>()
                    .ForMember(g => g.DateTime, o => o.MapFrom(c => DateTime.Now));
            });
        }

        private static void _seed(IApplicationBuilder app)
        {
            using (var db = app.ApplicationServices.GetService<BlogDbContext>())
            {
                //db.Database.Migrate();

                var userStore = new UserStore<User>(db);
                var roleStore = new RoleStore<IdentityRole>(db);
                
                if (!db.Roles.Any(r => r.Name == "Admin"))
                {
                    var r = roleStore.CreateAsync(new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" }).Result;
                    db.SaveChanges();
                }

                if (!db.Roles.Any(r => r.Name == "User"))
                {
                    var r = roleStore.CreateAsync(new IdentityRole() { Name = "User", NormalizedName = "USER" }).Result;
                    db.SaveChanges();
                }


                if (!userStore.Users.Any(u => u.UserName == "gagarin"))
                {
                    var user = new User
                    {
                        Id = Program.GagarinUserId,
                        UserName = "gagarin",
                        Email = "gagarinbefree@gmail.com",
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var password = new PasswordHasher<User>();
                    var hashed = password.HashPassword(user, "123");
                    user.PasswordHash = hashed;

                    var r2 = userStore.CreateAsync(user).Result;

                    db.SaveChanges();

                    String[] roles = { "Admin" };

                    UserManager<User> _userManager = app.ApplicationServices.GetService<UserManager<User>>();
                    var result = _userManager.AddToRolesAsync(user, roles).Result;                                      

                    db.SaveChanges();
                }                       
            }
        }
    }
}