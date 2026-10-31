using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TicketDto>> GetTickets()
    {
        var tickets = await _context
            .Tickets
            .Include(e => e.Employees)
            .Include(c => c.Categories)
            .ToListAsync();
        
        return tickets.Select(t => t.ToDto()).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetById(int id)
    {
        var ticket = await _context
            .Tickets
            .Include(e => e.Employees)
            .Include(c => c.Categories)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if (ticket == null) return NotFound();

        return Ok(ticket.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> Create(TicketDto dto)
    {
        var employees = await _context
            .Employees
            .Where(e => dto.EmployeeIds.Contains(e.Id))
            .ToListAsync();

        var categories = await _context
            .Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .ToListAsync();
        
        var ticket = new Tickets
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow,
            Employees = employees,
            Categories = categories
        };
        
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new {id = ticket.Id},
            dto
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TicketDto>> Update(int id, TicketRequest dto)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Employees)
            .Include(t => t.Categories)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return NotFound();
        
        var employees = await _context
            .Employees
            .Where(e => dto.EmployeeIds.Contains(e.Id))
            .ToListAsync();

        var categories = await _context
            .Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .ToListAsync();
        
        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Status = dto.Status;
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.Employees = employees;
        ticket.Categories = categories;
        
        await _context.SaveChangesAsync();  
        
        return Ok(ticket.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) 
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null) return NotFound();
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
