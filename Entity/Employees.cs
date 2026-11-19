namespace PracticeProject.Entity;

public class Employees
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Position { get; set; }
    public required string Password { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    
    public required string Role { get; set; }

    public ICollection<Categories> Categories { get; set; } = [];

    public ICollection<Tickets> Tickets { get; set; } = [];
}