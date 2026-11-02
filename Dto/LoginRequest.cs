namespace PracticeProject.Dto;

public record LoginRequest(
    string FullName,
    string Password
);

public record AuthUserDto(
    int Id,
    string Name,
    string Role
);

