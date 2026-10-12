using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TicketCategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketCategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TicketCategoryDto>> GetTicketCategories()
    {
        return await _context.TicketCategories.Select(tc => tc.ToDto()).ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketCategoryDto>> GetById(int id) {
        var ticketCategory = await _context.TicketCategories.FindAsync(id);

        if (ticketCategory == null) return NotFound();

        return Ok(ticketCategory.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TicketCategoryDto>> Create(TicketCategoryDto dto) {
        var ticketCategory = new TicketCategories {
            TicketId = dto.TicketId,
            CategoryId = dto.CategoryId,
        };

        await _context.TicketCategories.AddAsync(ticketCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new {id = dto.TicketId}, ticketCategory);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TicketCategoryDto>> Update(int id, TicketCategoryDto dto) {
        var ticketCategory = await _context.TicketCategories.FindAsync(id);

        if (ticketCategory == null) return NotFound();
        
        ticketCategory.CategoryId = dto.CategoryId;
        ticketCategory.TicketId = dto.TicketId;
        
        await _context.SaveChangesAsync();

        return Ok(ticketCategory.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) {
        var ticketCategory = await _context.TicketCategories.FindAsync(id);

        if (ticketCategory == null) return NotFound();

        _context.TicketCategories.Remove(ticketCategory);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
