using Microsoft.EntityFrameworkCore;
using VFX_Challenge.Models;

namespace VFX_Challenge.Repositories
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> GetExchangeRateAsync(string currencyPair);
        Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
        Task AddExchangeRateAsync(ExchangeRate rate);
        Task UpdateExchangeRateAsync(ExchangeRate rate);
        Task DeleteExchangeRateAsync(ExchangeRate rate);
    }

    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ExchangeRateDbContext _context;

        public ExchangeRateRepository(ExchangeRateDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma taxa de câmbio específica pelo par de moedas.
        /// </summary>
        public async Task<ExchangeRate> GetExchangeRateAsync(string currencyPair)
        {
            return await _context.ExchangeRates
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.CurrencyPair == currencyPair);
        }

        /// <summary>
        /// Obtém todas as taxas de câmbio armazenadas.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync()
        {
            return await _context.ExchangeRates
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Adiciona uma nova taxa de câmbio ao banco de dados.
        /// </summary>
        public async Task AddExchangeRateAsync(ExchangeRate rate)
        {
            _context.ExchangeRates.Add(rate);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza uma taxa de câmbio existente no banco de dados.
        /// </summary>
        public async Task UpdateExchangeRateAsync(ExchangeRate rate)
        {
            _context.ExchangeRates.Update(rate);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove uma taxa de câmbio do banco de dados.
        /// </summary>
        public async Task DeleteExchangeRateAsync(ExchangeRate rate)
        {
            _context.ExchangeRates.Remove(rate);
            await _context.SaveChangesAsync();
        }
    }
}
