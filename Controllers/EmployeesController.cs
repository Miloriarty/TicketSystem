using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Entity;
using PracticeProject.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Employees> _hasher;

    public EmployeesController(AppDbContext context, IPasswordHasher<Employees> hasher)
    {
        _context = context;
        _hasher = hasher;
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
    [Authorize(Roles="admin")]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        if (string.IsNullOrEmpty(dto.Password)) return BadRequest("Password is required");

        var categories = await _context
            .Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .ToListAsync();
        
        var date = DateTime.UtcNow;
        var employee = new Employees
        {
            FullName = dto.FullName,
            Position = dto.Position,
            Password = "",
            RoleId = dto.RoleId,
            StartDate = date,
            Categories = categories,
            Tickets = []
        };

        employee.Password = _hasher.HashPassword(employee, dto.Password);
        
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            employee.ToDto()
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles="admin")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, CreateEmployeeDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.Categories)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return NotFound();
        
        var categories = await _context
            .Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .ToListAsync();

       // var tickets = await _context
       //     .Tickets
       //     .Where(t => dto.TicketIds.Contains(t.Id))
       //     .ToListAsync();
        
        employee.FullName = dto.FullName;
        employee.Position = dto.Position;
        employee.RoleId = dto.RoleId;
        employee.Categories = categories;
        if (!string.IsNullOrEmpty(dto.Password)) employee.Password = _hasher.HashPassword(employee, dto.Password);
        //employee.Tickets = tickets;

        await _context.SaveChangesAsync();
        
        return Ok(employee.ToDto());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles="admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
