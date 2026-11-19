namespace PracticeProject.Auth;

public static class UserRoles
{
    public const string Admin = "admin";
    public const string Manager = "manager";
    public const string Employee = "employee";

    public static readonly IReadOnlyList<string> All = [Admin, Manager, Employee];

    public static bool IsValid(string? role) => role is not null && All.Contains(role);
}
