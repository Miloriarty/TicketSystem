namespace PracticeProject.Dto.Categories;

using PracticeProject.Entity;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Categories category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}