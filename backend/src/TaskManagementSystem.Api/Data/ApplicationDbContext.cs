using Microsoft.EntityFrameworkCore;
using TaskManagamentSystem.Api.Models;

namespace TaskManagamentSystem.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = default!;
    // code change: this model reference later
    public DbSet<Models.Task> Tasks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User settings
        modelBuilder.Entity<User>(x =>
        {
            x.HasIndex(user => user.Email).IsUnique();
            x.Property(user => user.Role).HasConversion<int>();
        });

        // Task settings
        modelBuilder.Entity<Models.Task>(x =>
        {
            x.Property(task => task.Status).HasConversion<int>();
            x.Property(task => task.taskPriority).HasConversion<int>();

            x.HasOne(task => task.Creator)
            .WithMany(user => user.CreatedTasks)
            .HasForeignKey(task => task.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

            x.HasOne(task => task.Assignee)
            .WithMany(user => user.AssignedTasks)
            .HasForeignKey(task => task.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);
        });

        base.OnModelCreating(modelBuilder);
    }
}