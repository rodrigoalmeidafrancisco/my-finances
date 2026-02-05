using System.Security.Cryptography;
using System.Text;
using MinhasFinancas.API.Models;

namespace MinhasFinancas.API.Services;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User?> RegisterAsync(string name, string email, string password);
    Task<User?> GetUserByIdAsync(int userId);
}

public class AuthService : IAuthService
{
    // In-memory user storage for demonstration purposes
    // In production, use a database like Entity Framework with SQL Server
    private static readonly List<User> _users = new();
    private static int _nextUserId = 1;

    public AuthService()
    {
        // Add a default user for testing if no users exist
        if (!_users.Any())
        {
            _users.Add(new User
            {
                Id = _nextUserId++,
                Name = "Usuario Demo",
                Email = "demo@minhasfinancas.com",
                PasswordHash = HashPassword("demo123"),
                CreatedAt = DateTime.UtcNow
            });
        }
    }

    public Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = _users.FirstOrDefault(u => 
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user == null)
            return Task.FromResult<User?>(null);

        var passwordHash = HashPassword(password);
        if (user.PasswordHash != passwordHash)
            return Task.FromResult<User?>(null);

        return Task.FromResult<User?>(user);
    }

    public Task<User?> RegisterAsync(string name, string email, string password)
    {
        // Check if user already exists
        if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult<User?>(null);

        var user = new User
        {
            Id = _nextUserId++,
            Name = name,
            Email = email,
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        _users.Add(user);
        return Task.FromResult<User?>(user);
    }

    public Task<User?> GetUserByIdAsync(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        return Task.FromResult(user);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
