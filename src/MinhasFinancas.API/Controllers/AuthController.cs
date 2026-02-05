using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasFinancas.API.Services;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email e senha são obrigatórios"
                });
            }

            var user = await _authService.AuthenticateAsync(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Email ou senha inválidos"
                });
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Success = true,
                Token = token,
                Message = "Login realizado com sucesso",
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Message = "Erro interno do servidor"
            });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Nome, email e senha são obrigatórios"
                });
            }

            var user = await _authService.RegisterAsync(request.Name, request.Email, request.Password);

            if (user == null)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email já está em uso"
                });
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Success = true,
                Token = token,
                Message = "Registro realizado com sucesso",
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Message = "Erro interno do servidor"
            });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserInfo>> GetCurrentUser()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário");
            return StatusCode(500);
        }
    }
}
