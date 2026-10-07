using PracticeProject.Entity;

namespace PracticeProject.Dto.EmployeeCategories;

public static class EmployeeCategoriesMapper
{
    public static EmployeeCategoriesDto ToDto(this Entity.EmployeeCategories employee)
    {
        return new EmployeeCategoriesDto
        {
            EmployeeId = employee.EmployeeId,
            CategoryId = employee.CategoryId,
        };
    }
}