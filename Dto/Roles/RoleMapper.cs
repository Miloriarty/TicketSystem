namespace PracticeProject.Dto.Roles;

using PracticeProject.Entity;

public static class RoleMapper
{
    public static RoleDto ToDto(this Roles role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };
    }
}