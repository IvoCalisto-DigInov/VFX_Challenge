using VFX_Challenge.Models;

namespace VFX_Challenge.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate> GetExchangeRateAsync(string BaseCurrency, string QuoteCurrency);
        Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
        Task<bool> AddExchangeRateAsync(ExchangeRate rate);
        Task<bool> UpdateExchangeRateAsync(string BaseCurrency, string QuoteCurrency, ExchangeRate updatedRate);
        Task<bool> DeleteExchangeRateAsync(string BaseCurrency, string QuoteCurrency);
    }
}
