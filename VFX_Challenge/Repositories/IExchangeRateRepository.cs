using VFX_Challenge.Models;

namespace VFX_Challenge.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> GetExchangeRateAsync(string BaseCurrency, string QuoteCurrency);
        Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
        Task AddExchangeRateAsync(ExchangeRate rate);
        Task UpdateExchangeRateAsync(ExchangeRate rate);
        Task DeleteExchangeRateAsync(ExchangeRate rate);
    }
}
