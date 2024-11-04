using VFX_Challenge.Models;

namespace VFX_Challenge.External
{
    public interface IExternalExchangeRateApi
    {
        Task<ExchangeRate> FetchExchangeRateAsync(string BaseCurrency, string QuoteCurrency);
    }
}
