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
        return await _context.Employees.Select(e => e.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        return Ok(employee.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(EmployeeDto employeeDto)
    {
        var employee = new Employees
        {
            FullName = employeeDto.FullName,
            Position = employeeDto.Position,
            RoleId = employeeDto.RoleId,
            StartDate = employeeDto.StartDate,
        };
        
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            employeeDto
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, EmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();
        
        employee.FullName = dto.FullName;
        employee.Position = dto.Position;
        employee.RoleId = dto.RoleId;

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