using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record TicketDto(
    int? Id,
    string Description,
    string Status,
    DateTime CreatedAt,
    ICollection<int> EmployeeIds,
    ICollection<int> CategoryIds
);

public record TicketRequest(
    string Description,
    string Status,
    ICollection<int> EmployeeIds,
    ICollection<int> CategoryIds
); // todo Ticket создается сразу с employees id, а employee может не принять заявку

public static class TicketMapper
{
    public static TicketDto ToDto(this Tickets ticket)
    {
        return new TicketDto(
            ticket.Id,
            ticket.Description,
            ticket.Status,
            ticket.CreatedAt,
            ticket.Employees.Select(e => e.Id).ToList(),
            ticket.Categories.Select(c => c.Id).ToList()
        );
    }
}
