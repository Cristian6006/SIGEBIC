namespace SIGEBIC.Domain.Interfaces;

public interface ICacheService
{
    Task SetAsync(string key, string value, TimeSpan expiration);
    Task<string?> GetAsync(string key);
    Task DeleteAsync(string key);
    Task<bool> ExistsAsync(string key);
}