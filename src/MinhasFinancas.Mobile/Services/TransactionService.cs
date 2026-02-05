using System.Net.Http.Json;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.Mobile.Services;

/// <summary>
/// Service for managing financial transactions through the API.
/// All requests include JWT token for authentication.
/// </summary>
public interface ITransactionService
{
    Task<List<TransactionDto>> GetAllAsync();
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto?> CreateAsync(TransactionDto transaction);
    Task<bool> UpdateAsync(int id, TransactionDto transaction);
    Task<bool> DeleteAsync(int id);
}

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authService;

    public TransactionService(HttpClient httpClient, IAuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<TransactionDto>> GetAllAsync()
    {
        await EnsureAuthenticatedAsync();
        try
        {
            var transactions = await _httpClient.GetFromJsonAsync<List<TransactionDto>>("api/transactions");
            return transactions ?? new List<TransactionDto>();
        }
        catch
        {
            return new List<TransactionDto>();
        }
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        await EnsureAuthenticatedAsync();
        try
        {
            return await _httpClient.GetFromJsonAsync<TransactionDto>($"api/transactions/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<TransactionDto?> CreateAsync(TransactionDto transaction)
    {
        await EnsureAuthenticatedAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/transactions", transaction);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TransactionDto>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, TransactionDto transaction)
    {
        await EnsureAuthenticatedAsync();
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/transactions/{id}", transaction);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await EnsureAuthenticatedAsync();
        try
        {
            var response = await _httpClient.DeleteAsync($"api/transactions/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task EnsureAuthenticatedAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
