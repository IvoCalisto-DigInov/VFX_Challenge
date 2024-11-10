using Microsoft.EntityFrameworkCore;
using VFX_Challenge.Models;

namespace VFX_Challenge.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ExchangeRateDbContext _context;
        private readonly ILogger<ExchangeRateRepository> _logger;

        public ExchangeRateRepository(ExchangeRateDbContext context, ILogger<ExchangeRateRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a specific exchange rate by currency pair from the database.
        /// </summary>
        /// <param name="BaseCurrency">The base currency code (e.g., USD).</param>
        /// <param name="QuoteCurrency">The quote currency code (e.g., EUR).</param>
        /// <returns>Returns the exchange rate if found, otherwise null.</returns>
        public async Task<ExchangeRate> GetExchangeRateAsync(string BaseCurrency, string QuoteCurrency)
        {
            _logger.LogInformation("Querying database for exchange rate: {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                return await _context.ExchangeRates
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.BaseCurrency == BaseCurrency && r.QuoteCurrency == QuoteCurrency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return null;
            }
        }

        /// <summary>
        /// Retrieves all stored exchange rates from the database.
        /// </summary>
        /// <returns>Returns a list of all exchange rates, or null if an error occurs.</returns>
        public async Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync()
        {
            _logger.LogInformation("Retrieving all exchange rates from the database.");
            try
            {
                return await _context.ExchangeRates
                    .AsNoTracking()
                    .ToListAsync();
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
        /// <param name="rate">The exchange rate object to add.</param>
        /// <returns>Returns true if the rate was added successfully, otherwise false.</returns>
        public async Task<bool> AddExchangeRateAsync(ExchangeRate rate)
        {
            _logger.LogInformation("Adding new exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
            try
            {
                _context.ExchangeRates.Add(rate);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Exchange rate added successfully.");
                return true;
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
        /// <param name="rate">The exchange rate object with updated information.</param>
        /// <returns>Returns true if the rate was updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateExchangeRateAsync(ExchangeRate rate)
        {
            _logger.LogInformation("Updating exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
            try
            {
                _context.ExchangeRates.Update(rate);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Exchange rate updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
                return false;
            }
        }

        /// <summary>
        /// Deletes an exchange rate from the database.
        /// </summary>
        /// <param name="rate">The exchange rate object to delete.</param>
        /// <returns>Returns true if the rate was deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteExchangeRateAsync(ExchangeRate rate)
        {
            _logger.LogInformation("Deleting exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
            try
            {
                _context.ExchangeRates.Remove(rate);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Exchange rate deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting exchange rate for {BaseCurrency}/{QuoteCurrency}", rate.BaseCurrency, rate.QuoteCurrency);
                return false;
            }
        }
    }
}
