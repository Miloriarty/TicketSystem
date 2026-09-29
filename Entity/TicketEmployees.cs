namespace PracticeProject.Entity;

public class TicketEmployees
{
    public int TicketId { get; set; }
    public int EmployeeId { get; set; }

    public Tickets Ticket { get; set; } = null!;
    public Employees Employee { get; set; } = null!;
}