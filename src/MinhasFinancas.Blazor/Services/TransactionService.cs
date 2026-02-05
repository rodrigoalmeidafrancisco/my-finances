using System.Net.Http.Json;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.Blazor.Services;

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

    public TransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TransactionDto>> GetAllAsync()
    {
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
}
