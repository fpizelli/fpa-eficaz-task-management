using EficazAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EficazAPI.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=app.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).HasConversion<string>();
                entity.Property(e => e.CreatedAt).IsRequired();
                
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CreatedAt);
                
                entity.Ignore("_domainEvents");
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UserId);
                entity.Property(e => e.Impact).IsRequired();
                entity.Property(e => e.Effort).IsRequired();
                entity.Property(e => e.Urgency).IsRequired();
                entity.Property(e => e.Score).IsRequired();
                
                entity.OwnsOne(e => e.Priority, priority =>
                {
                    priority.Property(p => p.Impact).HasColumnName("Impact");
                    priority.Property(p => p.Effort).HasColumnName("Effort");
                    priority.Property(p => p.Urgency).HasColumnName("Urgency");
                    priority.Property(p => p.Score).HasColumnName("Score");
                });
                
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Score);
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.Ignore("_domainEvents");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.TaskId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                
                entity.HasIndex(e => e.TaskId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.CreatedAt);
                
                entity.HasOne(e => e.Task)
                    .WithMany()
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.Ignore("_domainEvents");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.TaskId).IsRequired();
                entity.Property(e => e.Action)
                    .HasConversion(
                        v => v.Value,
                        v => EficazAPI.Domain.ValueObjects.AuditAction.Create(v))
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.OldValue).HasMaxLength(500);
                entity.Property(e => e.NewValue).HasMaxLength(500);
                entity.Property(e => e.UserId);
                
                entity.HasIndex(e => e.TaskId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.CreatedAt);
                
                entity.HasOne(e => e.Task)
                    .WithMany()
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
                    
            });
        }
    }
}
