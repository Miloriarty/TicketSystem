using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record TicketDto(
    int? Id,
    string Description,
    string Status,
    DateTime CreatedAt,
    int EmployeeId
);

public static class TicketMapper
{
    public static TicketDto ToDto(this Tickets ticket)
    {
        return new TicketDto(ticket.Id, ticket.Description, ticket.Status, ticket.CreatedAt, ticket.EmployeeId);
    }
}