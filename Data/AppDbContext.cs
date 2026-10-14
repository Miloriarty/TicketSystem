using Microsoft.EntityFrameworkCore;
using PracticeProject.Entity;

namespace PracticeProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categories> Categories => Set<Categories>();
    public DbSet<Employees> Employees => Set<Employees>();
    public DbSet<Roles> Roles => Set<Roles>();
    public DbSet<Tickets> Tickets => Set<Tickets>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employees -> Roles
        modelBuilder.Entity<Employees>()
            .HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RoleId);

        // Employees <-> Categories
        modelBuilder.Entity<Employees>()
            .HasMany(e => e.Categories)
            .WithMany(c => c.Employees);

        // Tickets <-> Categories
        modelBuilder.Entity<Tickets>()
            .HasMany(t => t.Categories)
            .WithMany(c => c.Tickets);

        // Tickets <-> Employees
        modelBuilder.Entity<Tickets>()
            .HasMany(t => t.Employees)
            .WithMany(e => e.Tickets);
    }
}