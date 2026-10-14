namespace PracticeProject.Entity;

public class Tickets
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; } // todo может заменить на enum
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Employees> Employees { get; set; } = [];
    public ICollection<Categories> Categories { get; set; } = [];
}