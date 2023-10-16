﻿using Explorer.Blog.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Blog.Infrastructure.Database;

public class BlogContext : DbContext
{
    public DbSet<BlogComment> BlogComments { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }

    public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("blog");

        ConfigureBlogComment(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users", schema: "stakeholders", t => t.ExcludeFromMigrations());
    }

    private static void ConfigureBlogComment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogComment>()
            .HasOne<BlogPost>()
            .WithMany()
            .HasForeignKey(bc => bc.BlogPostId);

        modelBuilder.Entity<BlogComment>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(bp => bp.UserId);
    }
}