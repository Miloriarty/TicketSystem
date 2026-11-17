using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet] // may be admin shouldn't see all tickets
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
    [Authorize(Roles="manager")]
    public async Task<ActionResult<TicketDto>> Create(TicketDto dto)
    {
        var employees = await _context
            .Employees
            .Include(e => e.Categories)
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
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            Employees = employees,
            Categories = categories
        };

        if (categories.Count != dto.CategoryIds.Distinct().Count()) {
            return BadRequest("Unknown category");
        }
        if (!IsAllCategoriesCovered(employees, categories)) {
            return BadRequest("Not all categories covered");
        }

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
                nameof(GetById),
                new {id = ticket.Id},
                ticket.ToDto()
                );
    }

    [HttpPut("{id}")]
    [Authorize(Roles="manager, employee")]
    public async Task<ActionResult<TicketDto>> Update(int id, TicketRequest dto)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Employees)
            .Include(t => t.Categories)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return NotFound();

        var employees = await _context
            .Employees
            .Include(e => e.Categories)
            .Where(e => dto.EmployeeIds.Contains(e.Id))
            .ToListAsync();

        var categories = await _context
            .Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .ToListAsync();

        if (User.IsInRole("employee")) {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!ticket.Employees.Any(e => e.Id == userId)) return Forbid();
        }
        
        if (categories.Count != dto.CategoryIds.Distinct().Count()) {
            return BadRequest("Unknown category");
        }
        if (!IsAllCategoriesCovered(employees, categories)) {
            return BadRequest("Not all categories covered");
        }

        ticket.Title = dto.Title;
        ticket.Description = dto.Description;
        ticket.Status = dto.Status;
        ticket.Employees = employees;
        ticket.Categories = categories;

        await _context.SaveChangesAsync();  

        return Ok(ticket.ToDto());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles="manager")]
    public async Task<ActionResult> Delete(int id) 
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null) return NotFound();
        
        if (ticket.Status == "New" || ticket.Status == "progress") {
            return BadRequest("Cant delete ticket because it is new or in progress");
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static bool IsAllCategoriesCovered(ICollection<Employees> employees, ICollection<Categories> categories) {
        if (categories.Count == 0) return false;

        var covered = employees
            .SelectMany(e => e.Categories)
            .Select(c => c.Id)
            .ToHashSet();

        return categories.All(c => covered.Contains(c.Id));
    }

}
