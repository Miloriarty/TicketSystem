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
    public DbSet<Tickets> Tickets => Set<Tickets>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employees -> Categories
        modelBuilder.Entity<Employees>()
            .HasMany(e => e.Categories)
            .WithMany()
            .UsingEntity(
                "CategoriesEmployees",
                r => r.HasOne(typeof(Categories)).WithMany().HasForeignKey("CategoriesId"),
                l => l.HasOne(typeof(Employees)).WithMany().HasForeignKey("EmployeesId"));

        // Tickets -> Categories
        modelBuilder.Entity<Tickets>()
            .HasMany(t => t.Categories)
            .WithMany()
            .UsingEntity(
                "CategoriesTickets",
                r => r.HasOne(typeof(Categories)).WithMany().HasForeignKey("CategoriesId"),
                l => l.HasOne(typeof(Tickets)).WithMany().HasForeignKey("TicketsId"));

        // Tickets <-> Employees
        modelBuilder.Entity<Tickets>()
            .HasMany(t => t.Employees)
            .WithMany(e => e.Tickets);
    }
}
