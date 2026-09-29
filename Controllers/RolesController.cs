using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Roles>> GetRoles() {
        return await _context.Roles.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Roles>> GetRole(int id) {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) return NotFound();

        return role;
    }

    [HttpPost]
    public async Task<ActionResult<Roles>> CreateRole(Roles role) {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetRole), 
            new {id = role.Id},
            role
        );
    }
}
