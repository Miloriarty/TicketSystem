using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record CategoryDto(
    int? Id,
    string Name,
    ICollection<int> EmployeeIds,
    ICollection<int> TicketIds
);

public record CreateCategoryRequest(
        string Name
);

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Categories category)
    {
        return new CategoryDto(
            category.Id,
            category.Name,
            category.Employees.Select(e => e.Id).ToList(),
            category.Tickets.Select(t => t.Id).ToList()
       );
    }
}
