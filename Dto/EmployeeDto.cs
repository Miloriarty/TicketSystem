using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record EmployeeDto(
    int? Id,
    string FullName,
    string Position,
    DateTime StartDate,
    int RoleId
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
            employee.RoleId
        );
    }
}