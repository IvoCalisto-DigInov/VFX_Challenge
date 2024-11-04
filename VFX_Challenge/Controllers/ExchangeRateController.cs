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
        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        /// <summary>
        /// Obtém a taxa de câmbio de um par de moedas específico.
        /// </summary>
        [HttpGet("{BaseCurrency}/{QuoteCurrency}")]
        public async Task<IActionResult> GetRate(string BaseCurrency, string QuoteCurrency)
        {
            try
            {
                var rate = await _exchangeRateService.GetExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (rate == null)
                {
                    return NotFound(new { Message = $"Exchange rate for {BaseCurrency}/{QuoteCurrency} not found." });
                }
                return Ok(rate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Obtém todas as taxas de câmbio disponíveis.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetAllRates()
        {
            try
            {
                var rates = await _exchangeRateService.GetAllExchangeRatesAsync();
                return Ok(rates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adiciona uma nova taxa de câmbio.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] ExchangeRate rate)
        {
            try
            {
                if (rate == null || string.IsNullOrEmpty(rate.BaseCurrency))
                {
                    return BadRequest(new { Message = "Invalid exchange rate data." });
                }

                await _exchangeRateService.AddExchangeRateAsync(rate);
                return CreatedAtAction(nameof(GetRate), new { currencyPair = rate.BaseCurrency }, rate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma taxa de câmbio existente.
        /// </summary>
        [HttpPut("{BaseCurrency}/{QuoteCurrency}")]
        public async Task<IActionResult> UpdateRate(string BaseCurrency, string QuoteCurrency, [FromBody] ExchangeRate updatedRate)
        {
            try
            {
                if (updatedRate == null || updatedRate.BaseCurrency != BaseCurrency)
                {
                    return BadRequest(new { Message = "Invalid exchange rate data." });
                }

                var result = await _exchangeRateService.UpdateExchangeRateAsync(BaseCurrency, QuoteCurrency, updatedRate);
                if (!result)
                {
                    return NotFound(new { Message = $"Exchange rate for {BaseCurrency}/{QuoteCurrency} not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Remove uma taxa de câmbio existente.
        /// </summary>
        [HttpDelete("{BaseCurrency}/{QuoteCurrency}")]
        public async Task<IActionResult> DeleteRate(string BaseCurrency, string QuoteCurrency)
        {
            try
            {
                var result = await _exchangeRateService.DeleteExchangeRateAsync(BaseCurrency, QuoteCurrency);
                if (!result)
                {
                    return NotFound(new { Message = $"Exchange rate for {BaseCurrency}/{QuoteCurrency} not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
