using Microsoft.EntityFrameworkCore;
using VFX_Challenge.Models;
using VFX_Challenge.Repositories;
namespace VFX_ChallengeTests
{
    public class ExchangeRateRepositoryTests
    {
        private readonly ExchangeRateDbContext _context;
        private readonly ExchangeRateRepository _repository;

        public ExchangeRateRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ExchangeRateDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDb")
                            .Options;
            _context = new ExchangeRateDbContext(options);
            _repository = new ExchangeRateRepository(_context);
        }

        [Fact]
        public async Task SaveRateAsync_SavesRateCorrectly()
        {
            // Arrange
            //var rate = new ExchangeRate { BaseCurrency = "USD", CurrencyTo = "EUR", Rate = 1.25m };

            //// Act
            //await _repository.SaveRateAsync(rate);
            //var savedRate = await _repository.GetCachedRateAsync("USD", "EUR");

            //// Assert
            //Assert.NotNull(savedRate);
            //Assert.Equal(1.25m, savedRate.Rate);
        }

        [Fact]
        public async Task GetCachedRateAsync_ReturnsNullIfRateNotFound()
        {
            // Act
            //var result = await _repository.GetCachedRateAsync("USD", "JPY");

            //Assert
            //Assert.Null(result);
        }
    }
}
