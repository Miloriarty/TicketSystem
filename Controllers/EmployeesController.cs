using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Entity;

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
    public async Task<IEnumerable<Employees>> GetEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employees>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employees>> CreateEmployee(Employees employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEmployees),
            new { id = employee.Id },
            employee
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employees employee)
    {
        if (id != employee.Id) return BadRequest();

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