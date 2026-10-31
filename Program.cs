using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
//
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.Cookie.Name = "ticket_auth";
//         options.Cookie.HttpOnly = true;
//         options.Cookie.SameSite = SameSiteMode.Lax;
//         options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
//         options.ExpireTimeSpan = TimeSpan.FromHours(8);
//         options.SlidingExpiration = true;
//
//         options.Events.OnRedirectToLogin = ctx =>
//         {
//             ctx.Response.StatusCode = 401;
//             return Task.CompletedTask;
//         };
//
//         options.Events.OnRedirectToAccessDenied = ctx =>
//         {
//             ctx.Response.StatusCode = 403;
//             return Task.CompletedTask;
//         };
//     });

// builder.Services.AddAuthorization(options =>
// {
//     options.FallBackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
// });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("Angular");
// app.UseAuthentication();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
