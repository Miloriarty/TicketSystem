using PracticeProject.Entity;

namespace PracticeProject.Dto;

public record EmployeeCategoryDto(int EmployeeId, int CategoryId);

public static class EmployeeCategoryMapper
{
    public static EmployeeCategoryDto ToDto(this Entity.EmployeeCategories employee)
    {
        return new EmployeeCategoryDto(employee.EmployeeId, employee.CategoryId);
    }
    
    public static EmployeeCategories FromDto(this EmployeeCategoryDto dto)
    {
        return new EmployeeCategories
        {
            EmployeeId = dto.EmployeeId,
            CategoryId = dto.CategoryId
        };
    }
}