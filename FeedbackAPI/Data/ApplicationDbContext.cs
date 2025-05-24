using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace FeedbackAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Feedback>()
            .HasKey(f => f.Id);

        modelBuilder.Entity<Feedback>()
            .Property(f => f.Sentimiento)
            .IsRequired()
            .HasMaxLength(10);

        modelBuilder.Entity<Feedback>()
            .Property(f => f.UserId)
            .IsRequired()
            .HasMaxLength(50);
    }
}
