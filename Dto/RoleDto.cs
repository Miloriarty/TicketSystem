using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record RoleDto(
    int? Id,
    string Name,
    ICollection<int> EmployeesId
    );

public static class RoleMapper
{
    public static RoleDto ToDto(this Roles role)
    {
        return new RoleDto(
            role.Id,
            role.Name,
            role.Employees.Select(e => e.Id).ToList()
        );
    }
}