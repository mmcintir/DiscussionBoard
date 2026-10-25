using DiscussionBoard.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<DiscussionThread> DiscussionThreads { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Post> Posts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DiscussionThread>()
                .HasOne(d => d.Forum)
                .WithMany(p => p.DiscussionThreads)
                .HasForeignKey(d => d.ForumId);
            builder.Entity<Post>()
                .HasOne(d => d.DiscussionThread)
                .WithMany(p => p.Posts)
                .HasForeignKey(d => d.DiscussionThreadId);
            builder.Entity<Post>()
                .HasOne(p => p.ParentPost)
                .WithMany(parent => parent.Replies)
                .HasForeignKey(p => p.ParentPostId);
            builder.Entity<DiscussionThread>()
                .HasOne(d => d.ApplicationUser)
                .WithMany(u => u.DiscussionThreads)
                .HasForeignKey(d => d.ApplicationUserId);
            builder.Entity<Post>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.ApplicationUserId);

        }
    }
}
