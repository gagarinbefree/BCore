using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCore;
using BCore.Dal.BlogModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BCore.Dal.Ef
{
    public class User : IdentityUser
    {        
    }

    public class BlogDbContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

