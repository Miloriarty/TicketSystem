using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase {
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> TryLogin(LoginRequest request) {
        var employee = await _context
            .Employees
            .Include(e => e.Categories)
            .Include(e => e.Tickets)
            .FirstOrDefaultAsync(e => e.FullName == request.FullName);

        if (employee == null) return NotFound();
        if (employee.Password == request.Password) {
            return Ok(employee.ToDto());
        }else {
            return Unauthorized();
        }
    }
}
