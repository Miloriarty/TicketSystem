namespace PracticeProject.Entity;

public class Categories
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Employees> Employees { get; set; } = [];
    public ICollection<Tickets> Tickets { get; set; } = [];
}