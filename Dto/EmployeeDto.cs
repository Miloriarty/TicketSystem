using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record EmployeeDto(
    int? Id,
    string FullName,
    string Position,
    int RoleId,
    ICollection<int> CategoriesId,
    ICollection<int> TicketsId
);

public record CreateEmployeeDto(
    string FullName,
    string Position,
    int RoleId,
    ICollection<int> CategoriesId
);

public static class EmployeeMapper
{
    public static EmployeeDto ToDto(this Employees employee)
    {
        return new EmployeeDto(
            employee.Id,
            employee.FullName,
            employee.Position,
            employee.RoleId,
            employee.Categories.Select(c => c.Id).ToList(),
            employee.Tickets.Select(t => t.Id).ToList()
        );
    }
}
