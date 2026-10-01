namespace PracticeProject.Dto.Employee;

using PracticeProject.Entity;


public static class EmployeeMapper
{
    public static EmployeeDto ToDto(this Employees employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Position = employee.Position,
            RoleId = employee.RoleId,
            StartDate = employee.StartDate,
        };
    }
}