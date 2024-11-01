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

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        /// <summary>
        /// Obt�m a taxa de c�mbio de um par de moedas espec�fico.
        /// </summary>
        [HttpGet("{currencyPair}")]
        public async Task<IActionResult> GetRate(string currencyPair)
        {
            var rate = await _exchangeRateService.GetExchangeRateAsync(currencyPair);
            if (rate == null)
            {
                return NotFound(new { Message = $"Exchange rate for {currencyPair} not found." });
            }
            return Ok(rate);
        }

        /// <summary>
        /// Obt�m todas as taxas de c�mbio dispon�veis.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetAllRates()
        {
            var rates = await _exchangeRateService.GetAllExchangeRatesAsync();
            return Ok(rates);
        }

        /// <summary>
        /// Adiciona uma nova taxa de c�mbio.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] ExchangeRate rate)
        {
            if (rate == null || string.IsNullOrEmpty(rate.CurrencyPair))
            {
                return BadRequest(new { Message = "Invalid exchange rate data." });
            }

            await _exchangeRateService.AddExchangeRateAsync(rate);
            return CreatedAtAction(nameof(GetRate), new { currencyPair = rate.CurrencyPair }, rate);
        }

        /// <summary>
        /// Atualiza uma taxa de c�mbio existente.
        /// </summary>
        [HttpPut("{currencyPair}")]
        public async Task<IActionResult> UpdateRate(string currencyPair, [FromBody] ExchangeRate updatedRate)
        {
            if (updatedRate == null || updatedRate.CurrencyPair != currencyPair)
            {
                return BadRequest(new { Message = "Invalid exchange rate data." });
            }

            var result = await _exchangeRateService.UpdateExchangeRateAsync(currencyPair, updatedRate);
            if (!result)
            {
                return NotFound(new { Message = $"Exchange rate for {currencyPair} not found." });
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma taxa de c�mbio existente.
        /// </summary>
        [HttpDelete("{currencyPair}")]
        public async Task<IActionResult> DeleteRate(string currencyPair)
        {
            var result = await _exchangeRateService.DeleteExchangeRateAsync(currencyPair);
            if (!result)
            {
                return NotFound(new { Message = $"Exchange rate for {currencyPair} not found." });
            }

            return NoContent();
        }
    }
}
