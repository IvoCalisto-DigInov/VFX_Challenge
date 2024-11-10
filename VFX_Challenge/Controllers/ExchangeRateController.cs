using Microsoft.AspNetCore.Mvc;
using VFX_Challenge.Models;
using VFX_Challenge.Services;

namespace VFX_Challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(IExchangeRateService exchangeRateService, ILogger<ExchangeRateController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the exchange rate for a specific currency pair.
        /// </summary>
        /// <param name="BaseCurrency">The base currency code (e.g., USD).</param>
        /// <param name="QuoteCurrency">The quote currency code (e.g., EUR).</param>
        /// <returns>Returns the exchange rate or a 404 if not found.</returns>
        [HttpGet("{BaseCurrency}/{QuoteCurrency}")]
        public async Task<IActionResult> GetRate(string BaseCurrency, string QuoteCurrency)
        {
            _logger.LogInformation("Request received for exchange rate: {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                var rate = await _exchangeRateService.GetExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (rate == null)
                {
                    _logger.LogWarning("Exchange rate for {BaseCurrency}/{QuoteCurrency} not found.", BaseCurrency, QuoteCurrency);
                    return NotFound(new { Message = $"Exchange rate for {BaseCurrency}/{QuoteCurrency} not found." });
                }
                _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} retrieved successfully.", BaseCurrency, QuoteCurrency);
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves all available exchange rates.
        /// </summary>
        /// <returns>Returns a list of all exchange rates.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetAllRates()
        {
            _logger.LogInformation("Request received to retrieve all exchange rates.");
            try
            {
                var rates = await _exchangeRateService.GetAllExchangeRatesAsync();
                _logger.LogInformation("All exchange rates retrieved successfully.");
                return Ok(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all exchange rates.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Adds a new exchange rate.
        /// </summary>
        /// <param name="rate">The exchange rate data to be added.</param>
        /// <returns>Returns the newly created exchange rate.</returns>
        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] ExchangeRate rate)
        {
            _logger.LogInformation("Request received to add a new exchange rate.");
            try
            {
                if (rate == null || string.IsNullOrEmpty(rate.BaseCurrency))
                {
                    _logger.LogWarning("Invalid exchange rate data received.");
                    return BadRequest(new { Message = "Invalid exchange rate data." });
                }

                await _exchangeRateService.AddExchangeRateAsync(rate);
                _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} added successfully.", rate.BaseCurrency, rate.QuoteCurrency);
                return CreatedAtAction(nameof(GetRate), new { currencyPair = rate.BaseCurrency }, rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new exchange rate.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Updates an existing exchange rate.
        /// </summary>
        /// <param name="BaseCurrency">The base currency of the rate to update.</param>
        /// <param name="QuoteCurrency">The quote currency of the rate to update.</param>
        /// <param name="updatedRate">The updated exchange rate details.</param>
        /// <returns>Returns a 204 if successful or 404 if not found.</returns>
        [HttpPut("{BaseCurrency}/{QuoteCurrency}")]
        public async Task<IActionResult> UpdateRate(string BaseCurrency, string QuoteCurrency, [FromBody] ExchangeRate updatedRate)
        {
            _logger.LogInformation("Request received to update exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                if (updatedRate == null || updatedRate.BaseCurrency != BaseCurrency)
                {
                    _logger.LogWarning("Invalid exchange rate data provided for update.");
                    return BadRequest(new { Message = "Invalid exchange rate data." });
                }

                var result = await _exchangeRateService.UpdateExchangeRateAsync(BaseCurrency, QuoteCurrency, updatedRate);
                if (!result)
                {
                    _logger.LogWarning("Exchange rate for {BaseCurrency}/{QuoteCurrency} not found.", BaseCurrency, QuoteCurrency);
                    return NotFound(new { Message = $"Exchange rate for {BaseCurrency}/{QuoteCurrency} not found." });
                }

                _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} updated successfully.", BaseCurrency, QuoteCurrency);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Removes an existing exchange rate.
        /// </summary>
        /// <param name="BaseCurrency">The base currency of the rate to delete.</param>
        /// <param name="QuoteCurrency">The quote currency of the rate to delete.</param>
        /// <returns>Returns a 204 if successful or 404 if not found.</returns>
        [HttpDelete("{BaseCurrency}/{QuoteCurrency}")]
        public async Task<IActionResult> DeleteRate(string BaseCurrency, string QuoteCurrency)
        {
            _logger.LogInformation("Request received to delete exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
            try
            {
                var result = await _exchangeRateService.DeleteExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (!result)
                {
                    _logger.LogWarning("Exchange rate for {BaseCurrency}/{QuoteCurrency} not found.", BaseCurrency, QuoteCurrency);
                    return NotFound(new { Message = $"Exchange rate for {BaseCurrency}/{QuoteCurrency} not found." });
                }

                _logger.LogInformation("Exchange rate for {BaseCurrency}/{QuoteCurrency} deleted successfully.", BaseCurrency, QuoteCurrency);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exchange rate for {BaseCurrency}/{QuoteCurrency}", BaseCurrency, QuoteCurrency);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
