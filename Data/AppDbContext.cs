using Microsoft.EntityFrameworkCore;
using PracticeProject.Entity;

namespace PracticeProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categories> Categories => Set<Categories>();
    public DbSet<EmployeeCategories> EmployeeCategories => Set<EmployeeCategories>();
    public DbSet<Employees> Employees => Set<Employees>();
    public DbSet<Roles> Roles => Set<Roles>();
    public DbSet<TicketCategories> TicketCategories => Set<TicketCategories>();
    public DbSet<TicketEmployees> TicketEmployees => Set<TicketEmployees>();
    public DbSet<Tickets> Tickets => Set<Tickets>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EmployeeCategories
        modelBuilder.Entity<EmployeeCategories>()
            .HasKey(x => new { x.EmployeeId, x.CategoryId });

        modelBuilder.Entity<EmployeeCategories>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.EmployeeCategories)
            .HasForeignKey(x => x.EmployeeId);

        modelBuilder.Entity<EmployeeCategories>()
            .HasOne(x => x.Category)
            .WithMany(x => x.EmployeeCategories)
            .HasForeignKey(x => x.CategoryId);
     
        // TicketCategories
        modelBuilder.Entity<TicketCategories>()
            .HasKey(x => new { x.TicketId, x.CategoryId });

        modelBuilder.Entity<TicketCategories>()
            .HasOne(x => x.Ticket)
            .WithMany(x => x.TicketCategories)
            .HasForeignKey(x => x.TicketId);

        modelBuilder.Entity<TicketCategories>()
            .HasOne(x => x.Category)
            .WithMany(x => x.TicketCategories)
            .HasForeignKey(x => x.CategoryId);

        // TicketEmployees
        modelBuilder.Entity<TicketEmployees>()
            .HasKey(x => new { x.TicketId, x.EmployeeId });

        modelBuilder.Entity<TicketEmployees>()
            .HasOne(x => x.Ticket)
            .WithMany(x => x.TicketEmployees)
            .HasForeignKey(x => x.TicketId);

        modelBuilder.Entity<TicketEmployees>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.TicketEmployees)
            .HasForeignKey(x => x.EmployeeId);

        // Employees -> Roles
        modelBuilder.Entity<Employees>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.RoleId);

        // Tickets -> Employees
        modelBuilder.Entity<Tickets>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.EmployeeId);
    }
}