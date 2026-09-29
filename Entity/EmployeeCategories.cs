namespace PracticeProject.Entity;

public class EmployeeCategories
{
    public int EmployeeId { get; set; }
    public int CategoryId { get; set; }

    public Employees Employee { get; set; } = null!;
    public Categories Category { get; set; } = null!;
}