namespace MinhasFinancas.Mobile.Services;

/// <summary>
/// Service for secure storage of sensitive data like authentication tokens.
/// In a real MAUI app, this would use SecureStorage.Default from MAUI.Essentials.
/// This interface allows for platform-specific implementations.
/// </summary>
public interface ISecureStorageService
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task RemoveAsync(string key);
    Task<bool> ContainsAsync(string key);
}

/// <summary>
/// In-memory implementation for demonstration.
/// In a real MAUI app, replace this with SecureStorage from MAUI.Essentials:
/// 
/// public class SecureStorageService : ISecureStorageService
/// {
///     public async Task<string?> GetAsync(string key)
///     {
///         return await SecureStorage.Default.GetAsync(key);
///     }
///
///     public async Task SetAsync(string key, string value)
///     {
///         await SecureStorage.Default.SetAsync(key, value);
///     }
///
///     public Task RemoveAsync(string key)
///     {
///         SecureStorage.Default.Remove(key);
///         return Task.CompletedTask;
///     }
///
///     public Task<bool> ContainsAsync(string key)
///     {
///         return Task.FromResult(SecureStorage.Default.GetAsync(key).Result != null);
///     }
/// }
/// </summary>
public class InMemorySecureStorageService : ISecureStorageService
{
    private readonly Dictionary<string, string> _storage = new();

    public Task<string?> GetAsync(string key)
    {
        _storage.TryGetValue(key, out var value);
        return Task.FromResult(value);
    }

    public Task SetAsync(string key, string value)
    {
        _storage[key] = value;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _storage.Remove(key);
        return Task.CompletedTask;
    }

    public Task<bool> ContainsAsync(string key)
    {
        return Task.FromResult(_storage.ContainsKey(key));
    }
}
