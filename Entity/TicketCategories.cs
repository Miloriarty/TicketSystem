namespace PracticeProject.Entity;

public class TicketCategories
{
    public int TicketId { get; set; }
    public int CategoryId { get; set; }

    public Tickets Ticket { get; set; } = null!;
    public Categories Category { get; set; } = null!;
}