namespace PracticeProject.Entity;

public class Roles
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Employees> Employees { get; set; } = [];
}