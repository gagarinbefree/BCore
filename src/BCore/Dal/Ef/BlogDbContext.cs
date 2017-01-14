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
        public DbSet<Hash> Hashes { set; get; }               
        public DbSet<PostHash> PostHashes { set; get; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PostHash>().HasKey(f => f.Id);
            
            builder.Entity<PostHash>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostHashes)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<PostHash>()
                .HasOne(pt => pt.Hash)
                .WithMany(t => t.PostHashes)
                .HasForeignKey(pt => pt.HashId);

            builder.Entity<Hash>().HasIndex(f => f.Tag).IsUnique();
            builder.Entity<Hash>().Property(f => f.Tag).IsRequired();

            builder.Entity<PartType>()
                .HasKey(e => e.Id);

            //builder.Entity<Part>()
            //    .HasOne(p => p.Type)
            //    .WithOne(i => i.Part)
            //    .HasForeignKey<PartType>(b => b.PartId);

            base.OnModelCreating(builder);
        }
    }
}

