using VFX_Challenge.Models;

namespace VFX_Challenge.External
{
    public class ExternalExchangeRateApi : IExternalExchangeRateApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalExchangeRateApi> _logger;

        public ExternalExchangeRateApi(HttpClient httpClient, ILogger<ExternalExchangeRateApi> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Fetches the exchange rate for a specified currency pair from an external API.
        /// </summary>
        /// <param name="BaseCurrency">The base currency code (e.g., USD).</param>
        /// <param name="QuoteCurrency">The quote currency code (e.g., EUR).</param>
        /// <returns>Returns an ExchangeRate object if successful, or null if an error occurs.</returns>
        public async Task<ExchangeRate> FetchExchangeRateAsync(string BaseCurrency, string QuoteCurrency)
        {
            _logger.LogInformation("Fetching exchange rate for {BaseCurrency}/{QuoteCurrency} from external API", BaseCurrency, QuoteCurrency);
            try
            {
                // TODO: Move API key to configuration for better security and maintainability
                string apiKey = "I5NQFWYWAYEUG9ED";
                var response = await _httpClient.GetAsync($"https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency={BaseCurrency}&to_currency={QuoteCurrency}&apikey={apiKey}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch exchange rate for {BaseCurrency}/{QuoteCurrency} - StatusCode: {StatusCode}", BaseCurrency, QuoteCurrency, response.StatusCode);
                    return null;
                }

                // Deserialize JSON response to RealtimeCurrencyExchangeRate model
                RealtimeCurrencyExchangeRate content = await response.Content.ReadFromJsonAsync<RealtimeCurrencyExchangeRate>();
                if (content != null)
                {
                    _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} successfully retrieved from API", BaseCurrency, QuoteCurrency);
                    // Map external response to internal ExchangeRate model
                    ExchangeRate exchangeRate = new ExchangeRate
                    {
                        Bid = content.Details.BidPrice,
                        Ask = content.Details.AskPrice,
                        BaseCurrency = BaseCurrency,
                        QuoteCurrency = QuoteCurrency,
                        Id = Guid.NewGuid()
                    };
                    return exchangeRate;
                }
                else
                {
                    _logger.LogWarning("No content found for exchange rate {BaseCurrency}/{QuoteCurrency} from API response", BaseCurrency, QuoteCurrency);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return null;
            }
        }
    }
}
