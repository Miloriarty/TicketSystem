using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto.EmployeeCategories;
using PracticeProject.Entity;


namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeCategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeCategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<EmployeeCategoriesDto>> GetEmployeeCategories()
    {
        return await _context.EmployeeCategories.Select(ec => ec.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeCategoriesDto>> GetById(int id)
    {
        var employeeCategory = await _context.EmployeeCategories.FindAsync(id);

        if (employeeCategory == null) return NotFound();

        return Ok(employeeCategory.ToDto());
    }
    
    [HttpPost]
    public async Task<ActionResult<EmployeeCategoriesDto>> Create(CreateEmployeeCategoriesDto dto)
    {
        var employeeCategory = new EmployeeCategories
        {
            EmployeeId = dto.EmployeeId,
            CategoryId = dto.CategoryId
        };

        _context.EmployeeCategories.Add(employeeCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = employeeCategory.EmployeeId },
            employeeCategory.ToDto()
        );
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<EmployeeCategoriesDto>> Update(int id, UpdateEmployeeCategoriesDto dto)
    {
        var employeeCategory = await _context.EmployeeCategories.FindAsync(id);

        if (employeeCategory == null) return NotFound();

        if (dto.CategoryId != null) employeeCategory.CategoryId = dto.CategoryId!.Value;

        await _context.SaveChangesAsync();

        return Ok(employeeCategory.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employeeCategory = await _context.EmployeeCategories.FindAsync(id);

        if (employeeCategory == null) return NotFound();

        _context.EmployeeCategories.Remove(employeeCategory);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
