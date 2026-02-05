using System.Net.Http.Headers;
using System.Net.Http.Json;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.Blazor.Services;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    string? GetToken();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string? _token;

    public AuthenticationService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                _token = result.Token;
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", _token);
            }

            return result ?? new LoginResponse { Success = false, Message = "Erro ao fazer login" };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = $"Erro: {ex.Message}" };
        }
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                _token = result.Token;
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", _token);
            }

            return result ?? new LoginResponse { Success = false, Message = "Erro ao registrar" };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = $"Erro: {ex.Message}" };
        }
    }

    public Task LogoutAsync()
    {
        _token = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return Task.CompletedTask;
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        if (string.IsNullOrEmpty(_token))
            return null;

        try
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>("api/auth/me");
        }
        catch
        {
            return null;
        }
    }

    public string? GetToken()
    {
        return _token;
    }
}
