// TaskService/Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoTask> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoTask>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<TodoTask>()
                .Property(t => t.Priority)
                .HasConversion<string>();
        }
    }
}