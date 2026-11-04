using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.Dto;
using PracticeProject.Entity;

namespace PracticeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LoginController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Employees> _hasher;

    public LoginController(AppDbContext context, IPasswordHasher<Employees> hasher) {
        _context = context;
        _hasher = hasher;
    }

    [HttpPost()]
    [AllowAnonymous]
    public async Task<ActionResult<AuthUserDto>> TryLogin(LoginRequest request) {
        var employee = await _context
            .Employees
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.FullName == request.FullName);

        if (employee == null) return NotFound();
        
        var result = _hasher.VerifyHashedPassword(employee, employee.Password, request.Password);
        if (result == PasswordVerificationResult.Failed) return Unauthorized();

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            employee.Password = _hasher.HashPassword(employee, request.Password);
            await _context.SaveChangesAsync();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new(ClaimTypes.Name, employee.FullName),
            new(ClaimTypes.Role, employee.Role.Name)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties {IsPersistent = true}
        );
        return Ok(new AuthUserDto(employee.Id, employee.FullName, employee.Role.Name));
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [HttpGet("me")]
    public ActionResult<AuthUserDto> Me() => Ok(new AuthUserDto(
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
        User.Identity!.Name!,
        User.FindFirstValue(ClaimTypes.Role)!
    ));
}
