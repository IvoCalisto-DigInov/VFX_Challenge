using System.Threading.Tasks;
using VFX_Challenge.Models;
namespace VFX_Challenge.External
{
    public interface IExternalExchangeRateApi
    {
        Task<ExchangeRate> FetchExchangeRateAsync(string currencyPair);
    }

    public class ExternalExchangeRateApi : IExternalExchangeRateApi
    {
        private readonly HttpClient _httpClient;

        public ExternalExchangeRateApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRate> FetchExchangeRateAsync(string currencyPair)
        {
            // Substitua pela URL da API real, utilizando a chave da API e parâmetros
            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/latest?base={currencyPair}");
            if (!response.IsSuccessStatusCode) return null;

            var data = await response.Content.ReadFromJsonAsync<ExchangeRate>();
            return data;
        }
    }
}