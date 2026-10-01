namespace PracticeProject.Dto.Tickets;

public class UpdateTicketDto
{
    public string? Description { get; set; }
    public string? Status { get; set; } // todo заменить на enum
    
    public int? EmployeeId { get; set; }
}