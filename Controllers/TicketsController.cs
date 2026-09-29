using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
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
    public async Task<IEnumerable<Tickets>> GetTickets() {
        return await _context.Tickets.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tickets>> GetTicket(int id) {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null) return NotFound();

        return ticket;
    }

    [HttpPost]
    public async Task<ActionResult<Tickets>> CreateTicket(Tickets ticket) {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
                nameof(GetTicket),
                new {id = ticket.Id},
                ticket
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Tickets>> UpdateTicket(int id, Tickets ticket) {
        if (id != ticket.Id) return BadRequest();

        _context.Tickets.Entry(ticket).State = EntityState.Modified;  
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Tickets>> DeleteTicket(int id) {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null) return NotFound();
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
