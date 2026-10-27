using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<CategoryDto>> GetCategories()
    {
        return await _context.Categories.Select(c => c.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null) return NotFound();

        return Ok(category.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CategoryDto categoryDto)
    {
        var category = new Categories
        {
            Name = categoryDto.Name
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            categoryDto
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, CreateCategoryRequest dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        
        category.Name = dto.Name;
        
        await _context.SaveChangesAsync();
        
        return Ok(category.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
