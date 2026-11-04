using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;
using Microsoft.AspNetCore.Authorization;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    [Authorize(Roles="admin")]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryRequest dto)
    {
        var category = new Categories
        {
            Name = dto.Name
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category.ToDto()
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles="admin")]
    public async Task<ActionResult<CategoryDto>> Update(int id, CreateCategoryRequest dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        category.Name = dto.Name;

        await _context.SaveChangesAsync();

        return Ok(category.ToDto());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles="admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
