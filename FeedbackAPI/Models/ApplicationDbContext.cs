using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace FeedbackAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PostId).IsRequired();
            entity.Property(e => e.Sentimiento).IsRequired();
            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).IsRequired();

            // Índice único para evitar múltiples votos del mismo usuario en el mismo post
            entity.HasIndex(e => new { e.PostId, e.UserId }).IsUnique();
        });
    }
}
