namespace PracticeProject.Entity;

public class Employees
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Position { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    
    public required int RoleId { get; set; }
    public Roles Role { get; set; } = null!;

    public ICollection<EmployeeCategories> EmployeeCategories { get; set; } = [];
    public ICollection<TicketEmployees> TicketEmployees { get; set; } = [];
    public ICollection<Tickets> Tickets { get; set; } = [];
}