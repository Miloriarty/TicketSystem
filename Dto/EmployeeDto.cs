using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record EmployeeDto(
    int? Id,
    string FullName,
    string Position,
    DateTime StartDate,
    int RoleId,
    ICollection<int> CategoriesId,
    ICollection<int> TicketsId
);

public static class EmployeeMapper
{
    public static EmployeeDto ToDto(this Employees employee)
    {
        return new EmployeeDto(
            employee.Id,
            employee.FullName,
            employee.Position,
            employee.StartDate,
            employee.RoleId,
            employee.Categories.Select(c => c.Id).ToList(),
            employee.Tickets.Select(t => t.Id).ToList()
        );
    }
}