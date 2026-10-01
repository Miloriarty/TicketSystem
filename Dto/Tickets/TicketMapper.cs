namespace PracticeProject.Dto.Tickets;
using PracticeProject.Entity;

public static class TicketMapper
{
    public static TicketDto ToDto(this Tickets ticket)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            EmployeeId = ticket.EmployeeId,
            Status = ticket.Status,
        };
    }
}