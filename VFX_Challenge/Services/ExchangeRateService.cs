using VFX_Challenge.Models;
using VFX_Challenge.Repositories;
using VFX_Challenge.External;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VFX_Challenge.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _repository;
        private readonly IExternalExchangeRateApi _externalApi;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(IExchangeRateRepository repository, IExternalExchangeRateApi externalApi, ILogger<ExchangeRateService> logger)
        {
            _repository = repository;
            _externalApi = externalApi;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the exchange rate for a specific currency pair.
        /// Checks the database first; if not found, fetches from an external API and stores it.
        /// </summary>
        /// <param name="BaseCurrency">The base currency code (e.g., USD).</param>
        /// <param name="QuoteCurrency">The quote currency code (e.g., EUR).</param>
        /// <returns>Returns the exchange rate object, or null if not found.</returns>
        public async Task<ExchangeRate> GetExchangeRateAsync(string BaseCurrency, string QuoteCurrency)
        {
            _logger.LogInformation("Attempting to retrieve exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                // Check if the rate is in the database
                var rate = await _repository.GetExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (rate == null)
                {
                    _logger.LogInformation("Exchange rate not found in database. Fetching from external API.");
                    // Fetch from external API if not in database
                    rate = await _externalApi.FetchExchangeRateAsync(BaseCurrency, QuoteCurrency);
                    if (rate != null)
                    {
                        _logger.LogInformation("Exchange rate fetched from external API. Storing in database.");
                        await _repository.AddExchangeRateAsync(rate);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to retrieve exchange rate for {BaseCurrency}/{QuoteCurrency} from external API.", BaseCurrency, QuoteCurrency);
                    }
                }
                return rate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return null;
            }
        }

        /// <summary>
        /// Retrieves all available exchange rates from the database.
        /// </summary>
        /// <returns>Returns a list of all exchange rates, or null if an error occurs.</returns>
        public async Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync()
        {
            _logger.LogInformation("Attempting to retrieve all exchange rates from the database.");
            try
            {
                return await _repository.GetAllExchangeRatesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all exchange rates.");
                return null;
            }
        }

        /// <summary>
        /// Adds a new exchange rate to the database.
        /// </summary>
        /// <param name="rate">The exchange rate object to be added.</param>
        /// <returns>Returns true if the rate was added successfully, false otherwise.</returns>
        public async Task<bool> AddExchangeRateAsync(ExchangeRate rate)
        {
            _logger.LogInformation("Attempting to add a new exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
            try
            {
                return await _repository.AddExchangeRateAsync(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
                return false;
            }
        }

        /// <summary>
        /// Updates an existing exchange rate in the database.
        /// </summary>
        /// <param name="BaseCurrency">The base currency of the rate to update.</param>
        /// <param name="QuoteCurrency">The quote currency of the rate to update.</param>
        /// <param name="updatedRate">The updated exchange rate details.</param>
        /// <returns>Returns true if the rate was updated successfully, false otherwise.</returns>
        public async Task<bool> UpdateExchangeRateAsync(string BaseCurrency, string QuoteCurrency, ExchangeRate updatedRate)
        {
            _logger.LogInformation("Attempting to update exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                var existingRate = await _repository.GetExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (existingRate == null)
                {
                    _logger.LogWarning("Exchange rate for {BaseCurrency}/{QuoteCurrency} not found for update.", BaseCurrency, QuoteCurrency);
                    return false;
                }

                // Update bid and ask values
                existingRate.Bid = updatedRate.Bid;
                existingRate.Ask = updatedRate.Ask;

                await _repository.UpdateExchangeRateAsync(existingRate);
                _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} updated successfully.", BaseCurrency, QuoteCurrency);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return false;
            }
        }

        /// <summary>
        /// Removes an exchange rate from the database.
        /// </summary>
        /// <param name="BaseCurrency">The base currency of the rate to delete.</param>
        /// <param name="QuoteCurrency">The quote currency of the rate to delete.</param>
        /// <returns>Returns true if the rate was deleted successfully, false otherwise.</returns>
        public async Task<bool> DeleteExchangeRateAsync(string BaseCurrency, string QuoteCurrency)
        {
            _logger.LogInformation("Attempting to delete exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                var existingRate = await _repository.GetExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (existingRate == null)
                {
                    _logger.LogWarning("Exchange rate for {BaseCurrency}/{QuoteCurrency} not found for deletion.", BaseCurrency, QuoteCurrency);
                    return false;
                }

                await _repository.DeleteExchangeRateAsync(existingRate);
                _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} deleted successfully.", BaseCurrency, QuoteCurrency);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return false;
            }
        }
    }
}
