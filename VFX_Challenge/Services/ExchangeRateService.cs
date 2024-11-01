
using VFX_Challenge.Models;
using VFX_Challenge.Repositories;
using VFX_Challenge.External;

namespace VFX_Challenge.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate> GetExchangeRateAsync(string currencyPair);
        Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
        Task AddExchangeRateAsync(ExchangeRate rate);
        Task<bool> UpdateExchangeRateAsync(string currencyPair, ExchangeRate updatedRate);
        Task<bool> DeleteExchangeRateAsync(string currencyPair);
    }

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _repository;
        private readonly IExternalExchangeRateApi _externalApi;

        public ExchangeRateService(IExchangeRateRepository repository, IExternalExchangeRateApi externalApi)
        {
            _repository = repository;
            _externalApi = externalApi;
        }

        /// <summary>
        /// Obtém a taxa de câmbio de um par de moedas específico.
        /// Se não estiver no banco de dados, busca na API externa e armazena.
        /// </summary>
        public async Task<ExchangeRate> GetExchangeRateAsync(string currencyPair)
        {
            // Verifica se a taxa está no banco de dados
            var rate = await _repository.GetExchangeRateAsync(currencyPair);
            if (rate == null)
            {
                // Busca a taxa na API externa
                rate = await _externalApi.FetchExchangeRateAsync(currencyPair);
                if (rate != null)
                {
                    // Armazena no banco de dados se a taxa foi obtida da API externa
                    await _repository.AddExchangeRateAsync(rate);
                }
            }
            return rate;
        }

        /// <summary>
        /// Obtém todas as taxas de câmbio disponíveis no banco de dados.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync()
        {
            return await _repository.GetAllExchangeRatesAsync();
        }

        /// <summary>
        /// Adiciona uma nova taxa de câmbio ao banco de dados.
        /// </summary>
        public async Task AddExchangeRateAsync(ExchangeRate rate)
        {
            await _repository.AddExchangeRateAsync(rate);
            // Adicione aqui a lógica para disparar um evento, se necessário
        }

        /// <summary>
        /// Atualiza uma taxa de câmbio existente no banco de dados.
        /// </summary>
        public async Task<bool> UpdateExchangeRateAsync(string currencyPair, ExchangeRate updatedRate)
        {
            var existingRate = await _repository.GetExchangeRateAsync(currencyPair);
            if (existingRate == null)
            {
                return false;
            }

            // Atualiza os valores do bid e ask
            existingRate.Bid = updatedRate.Bid;
            existingRate.Ask = updatedRate.Ask;

            await _repository.UpdateExchangeRateAsync(existingRate);
            return true;
        }

        /// <summary>
        /// Remove uma taxa de câmbio do banco de dados.
        /// </summary>
        public async Task<bool> DeleteExchangeRateAsync(string currencyPair)
        {
            var existingRate = await _repository.GetExchangeRateAsync(currencyPair);
            if (existingRate == null)
            {
                return false;
            }

            await _repository.DeleteExchangeRateAsync(existingRate);
            return true;
        }
    }
}
