using System.Net.Http.Headers;
using System.Net.Http.Json;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.Mobile.Services;

/// <summary>
/// Service for handling authentication with the API using JWT tokens.
/// This service manages login, registration, and token storage for the mobile app.
/// </summary>
public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ISecureStorageService _secureStorage;
    private const string TokenKey = "auth_token";

    public AuthenticationService(HttpClient httpClient, ISecureStorageService secureStorage)
    {
        _httpClient = httpClient;
        _secureStorage = secureStorage;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await _secureStorage.SetAsync(TokenKey, result.Token);
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", result.Token);
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
                await _secureStorage.SetAsync(TokenKey, result.Token);
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", result.Token);
            }

            return result ?? new LoginResponse { Success = false, Message = "Erro ao registrar" };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = $"Erro: {ex.Message}" };
        }
    }

    public async Task LogoutAsync()
    {
        await _secureStorage.RemoveAsync(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<UserInfo>("api/auth/me");
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _secureStorage.GetAsync(TokenKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}
