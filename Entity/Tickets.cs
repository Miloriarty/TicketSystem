namespace PracticeProject.Entity;

public class Tickets
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; } // todo может заменить на enum
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int EmployeeId { get; set; }
    public Employees Employee { get; set; } = null!;

    public ICollection<TicketCategories> TicketCategories { get; set; } = null!;
    public ICollection<TicketEmployees> TicketEmployees { get; set; } = null!;  
}