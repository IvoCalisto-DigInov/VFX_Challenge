using VFX_Challenge.Models;
namespace VFX_Challenge.External
{
    public class ExternalExchangeRateApi : IExternalExchangeRateApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalExchangeRateApi> _logger;
        public ExternalExchangeRateApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRate> FetchExchangeRateAsync(string BaseCurrency, string QuoteCurrency)
        {
            try
            {
                ExchangeRate exchangeRate = null;
                string apiKey = "I5NQFWYWAYEUG9ED";
                // Substitua pela URL da API real, utilizando a chave da API e parâmetros
                var response = await _httpClient.GetAsync($"https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency={BaseCurrency}&to_currency={QuoteCurrency}&apikey={apiKey}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                RealtimeCurrencyExchangeRate data = await response.Content.ReadFromJsonAsync<RealtimeCurrencyExchangeRate>();
                if(data != null)
                {
                    exchangeRate = new ExchangeRate()
                    {
                        Bid = data.Details.BidPrice,
                        Ask = data.Details.AskPrice,
                        BaseCurrency = BaseCurrency,
                        QuoteCurrency = QuoteCurrency,
                        Id = new Guid()
                    };
                }
                return exchangeRate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}