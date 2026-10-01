namespace PracticeProject.Dto.Tickets;

public class CreateTicketDto
{
    public required string Description { get; set; }
    public required string Status { get; set; } // todo заменить на enum
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int EmployeeId { get; set; }
}