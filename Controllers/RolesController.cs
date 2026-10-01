using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto.Roles;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context) 
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Roles>> GetRoles() 
    {
        return await _context.Roles.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(int id) 
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) return NotFound();

        return Ok(role.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(CreateRoleDto dto)
    {
        var role = new Roles
        {
            Name = dto.Name
        };
        
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById), 
            new {id = role.Id},
            dto
        );
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<RoleDto>> Update(int id, UpdateRoleDto dto)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) return NotFound();

        if (dto.Name is not null) role.Name = dto.Name;

        _context.Roles.Entry(role).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return Ok(role.ToDto());
    }
}
