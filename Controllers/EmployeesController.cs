using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Entity;
using PracticeProject.Dto.Employee;

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
    public async Task<ActionResult<Employees>> GetById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employees>> Create(CreateEmployeeDto employeeDto)
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
    public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        if (dto.FullName is not null) employee.FullName = dto.FullName;
        if (dto.Position is not null) employee.Position = dto.Position;
        if (dto.RoleId is not null) employee.RoleId = dto.RoleId!.Value;

        _context.Employees.Entry(employee).State = EntityState.Modified;
        _context.Employees.Update(employee);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}