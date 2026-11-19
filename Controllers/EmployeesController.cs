using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Entity;
using PracticeProject.Dto;
using PracticeProject.Auth;
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
    [Authorize(Roles = UserRoles.Manager + "," + UserRoles.Admin)]
    public async Task<IEnumerable<EmployeeDto>> GetEmployees()
    {
        var query = _context
            .Employees
            .Include(e => e.Categories)
            .Include(t => t.Tickets)
            .AsQueryable();

        if (User.IsInRole(UserRoles.Manager)) {
            query = query.Where(e => e.Role == UserRoles.Employee);
        }
        
        return await query.Select(e => e.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Manager + "," + UserRoles.Admin)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _context
            .Employees
            .Include(e => e.Categories)
            .Include(t => t.Tickets)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if (employee == null) return NotFound();
        if (User.IsInRole(UserRoles.Manager)) {
            if (employee.Role != UserRoles.Employee) return Forbid();
        }

        return Ok(employee.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        if (string.IsNullOrEmpty(dto.Password)) return BadRequest("Password is required");
        if (!UserRoles.IsValid(dto.Role)) return BadRequest("Unknown role");

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
            Role = dto.Role,
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
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, CreateEmployeeDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.Categories)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return NotFound();
        if (!UserRoles.IsValid(dto.Role)) return BadRequest("Unknown role");
        
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
        employee.Role = dto.Role;
        employee.Categories = categories;
        if (!string.IsNullOrEmpty(dto.Password)) employee.Password = _hasher.HashPassword(employee, dto.Password);
        //employee.Tickets = tickets;

        await _context.SaveChangesAsync();
        
        return Ok(employee.ToDto());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
