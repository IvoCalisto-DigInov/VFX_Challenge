using VFX_Challenge.Models;

namespace VFX_Challenge.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> GetExchangeRateAsync(string BaseCurrency, string QuoteCurrency);
        Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
        Task<bool> AddExchangeRateAsync(ExchangeRate rate);
        Task<bool> UpdateExchangeRateAsync(ExchangeRate rate);
        Task<bool> DeleteExchangeRateAsync(ExchangeRate rate);
    }
}
