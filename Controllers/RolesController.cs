using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
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
    public async Task<IEnumerable<RoleDto>> GetRoles() 
    {
        return await _context.Roles.Select(r => r.ToDto()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(int id) 
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) return NotFound();

        return Ok(role.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(RoleDto dto)
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

    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> Update(int id, RoleDto dto)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) return NotFound();
        
        role.Name = dto.Name;
        
        await _context.SaveChangesAsync();
        return Ok(role.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) return NotFound();

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
