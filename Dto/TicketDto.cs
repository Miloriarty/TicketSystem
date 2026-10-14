using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record TicketDto(
    int? Id,
    string Description,
    string Status,
    DateTime CreatedAt,
    ICollection<int> EmployeesId,
    ICollection<int> CategoriesId
);

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