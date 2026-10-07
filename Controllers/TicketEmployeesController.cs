using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto.TicketEmployees;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketEmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketEmployeesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TicketEmployeesDto>> GetTicketEmployees()
    {
        return await _context.TicketEmployees.Select(te => te.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketEmployeesDto>> GetById(int id)
    {
        var ticketEmployee = await _context.TicketEmployees.FindAsync(id);

        if (ticketEmployee == null) return NotFound();

        return Ok(ticketEmployee.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TicketEmployeesDto>> Create(CreateTicketEmployeeDto dto)
    {
        var ticketEmployee = new TicketEmployees
        {
            EmployeeId = dto.EmployeeId,
            TicketId = dto.TicketId,
        };

        _context.TicketEmployees.Add(ticketEmployee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticketEmployee.EmployeeId },
            ticketEmployee.ToDto()
        );
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var ticketEmployee = await _context.TicketEmployees.FindAsync(id);
        
        if (ticketEmployee == null) return NotFound();
        
        _context.TicketEmployees.Remove(ticketEmployee);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
}