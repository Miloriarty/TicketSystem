using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Entity;
using PracticeProject.Dto;

namespace PracticeProject.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetEmployees()
    {
        var employees = await _context
            .Employees
            .Include(e => e.Categories)
            .Include(t => t.Tickets)
            .ToListAsync();
        
        return employees.Select(e => e.ToDto()).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _context
            .Employees
            .Include(e => e.Categories)
            .Include(t => t.Tickets)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if (employee == null) return NotFound();

        return Ok(employee.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var categories = await _context
            .Categories
            .Where(c => dto.CategoriesId.Contains(c.Id))
            .ToListAsync();
        
        var date = DateTime.UtcNow;
        var employee = new Employees
        {
            FullName = dto.FullName,
            Position = dto.Position,
            RoleId = dto.RoleId,
            StartDate = date,
            Categories = categories,
            Tickets = []
        };
        
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            dto
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, CreateEmployeeDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.Categories)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return NotFound();
        
        var categories = await _context
            .Categories
            .Where(c => dto.CategoriesId.Contains(c.Id))
            .ToListAsync();

       // var tickets = await _context
       //     .Tickets
       //     .Where(t => dto.TicketsId.Contains(t.Id))
       //     .ToListAsync();
        
        employee.FullName = dto.FullName;
        employee.Position = dto.Position;
        employee.RoleId = dto.RoleId;
        employee.Categories = categories;
        //employee.Tickets = tickets;

        await _context.SaveChangesAsync();
        
        return Ok(employee.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
