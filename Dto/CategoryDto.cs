using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record CategoryDto(int? Id, string Name);

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Categories category)
    {
        return new CategoryDto(category.Id, category.Name);
    }
}
