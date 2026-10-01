namespace PracticeProject.Dto.Employee;

public class EmployeeDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Position { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    
    public int RoleId { get; set; }
}