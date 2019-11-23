using DatingApp.API.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class DatingAppContext : IdentityDbContext<User,Role,int,IdentityUserClaim<int>,
        UserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DatingAppContext(DbContextOptions<DatingAppContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Like>().HasKey(l => new { l.LikerId, l.LikeeId });

            modelBuilder.Entity<Like>().HasOne(l => l.Liker).WithMany(u => u.Likees).
                HasForeignKey(l => l.LikerId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Like>().HasOne(l => l.Likee).WithMany(u => u.Likers).
                HasForeignKey(l => l.LikeeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>().HasOne(l => l.Sender).WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>().HasOne(l => l.Recipient).WithMany(u => u.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);

            modelBuilder.Entity<UserRole>(userRole => {
                userRole.HasKey(k => new { k.UserId, k.RoleId });
                userRole.HasOne(l => l.Role).WithMany(u => u.UserRoles).HasForeignKey(l=>l.RoleId)
                .IsRequired();
                userRole.HasOne(l => l.User).WithMany(u => u.UserRoles).HasForeignKey(l=>l.UserId).IsRequired();
            });

        }
        public DbSet<Value> Values { get; set; } 
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Message { get; set; }
    }
}
