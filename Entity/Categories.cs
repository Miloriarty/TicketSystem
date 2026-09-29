namespace PracticeProject.Entity;

public class Categories
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<EmployeeCategories> EmployeeCategories { get; set; } = [];
    public ICollection<TicketCategories> TicketCategories { get; set; } = [];
}