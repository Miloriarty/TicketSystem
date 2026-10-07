using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto.Tickets;
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
        return await _context.Tickets.Select(t => t.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetById(int id) 
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null) return NotFound();

        return Ok(ticket.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> Create(CreateTicketDto dto)
    {
        var ticket = new Tickets
        {
            Description = dto.Description,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow,
            EmployeeId = dto.EmployeeId,
        };
        
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new {id = ticket.Id},
            dto
        );
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TicketDto>> Update(int id, UpdateTicketDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();
        
        if (dto.Description is not null) ticket.Description = dto.Description;
        if (dto.Status is not null) ticket.Status = dto.Status;
        if (dto.EmployeeId is not null)  ticket.EmployeeId = dto.EmployeeId!.Value;
        
        _context.Tickets.Entry(ticket).State = EntityState.Modified;
        _context.Tickets.Update(ticket);
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
