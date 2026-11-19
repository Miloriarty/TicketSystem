using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record EmployeeDto(
    int? Id,
    string FullName,
    string Position,
    string Role,
    DateTime startDate,
    ICollection<int> CategoryIds,
    ICollection<int> TicketIds
);

public record CreateEmployeeDto(
    string FullName,
    string Position,
    string? Password,
    string Role,
    ICollection<int> CategoryIds
);

public static class EmployeeMapper
{
    public static EmployeeDto ToDto(this Employees employee)
    {
        return new EmployeeDto(
            employee.Id,
            employee.FullName,
            employee.Position,
            employee.Role,
            employee.StartDate,
            employee.Categories.Select(c => c.Id).ToList(),
            employee.Tickets.Select(t => t.Id).ToList()
        );
    }
}
