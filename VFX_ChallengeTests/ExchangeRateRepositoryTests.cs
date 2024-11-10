using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using VFX_Challenge.Models;
using VFX_Challenge.Repositories;

namespace VFX_ChallengeTests
{
    public class ExchangeRateRepositoryTests
    {
        private readonly ExchangeRateDbContext _context;
        private readonly Mock<ILogger<ExchangeRateRepository>> _loggerMock;
        private readonly ExchangeRateRepository _repository;

        public ExchangeRateRepositoryTests()
        {
            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ExchangeRateDbContext>()
                .UseInMemoryDatabase(databaseName: "ExchangeRateTestDb")
                .Options;
            _context = new ExchangeRateDbContext(options);

            // Initialize the logger mock and repository instance
            _loggerMock = new Mock<ILogger<ExchangeRateRepository>>();
            _repository = new ExchangeRateRepository(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRate_WhenExistsInDatabase()
        {
            // Arrange
            _context.ExchangeRates.RemoveRange(_context.ExchangeRates);
            _context.SaveChanges();

            var expectedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            await _context.ExchangeRates.AddAsync(expectedRate);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Equivalent(expectedRate, result);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_WhenNotInDatabase()
        {
            //Arrange 
            _context.ExchangeRates.RemoveRange(_context.ExchangeRates);
            await _context.SaveChangesAsync();
            // Act
            var result = await _repository.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllExchangeRatesAsync_ReturnsAllRates()
        {
            // Arrange
            _context.ExchangeRates.RemoveRange(_context.ExchangeRates);
            await _context.SaveChangesAsync();

            var rates = new List<ExchangeRate>
            {
                new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR" },
                new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "GBP" }
            };
            await _context.ExchangeRates.AddRangeAsync(rates);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllExchangeRatesAsync();

            // Assert
            Assert.Equivalent(rates, result);
        }

        [Fact]
        public async Task AddExchangeRateAsync_AddsRateToDatabase()
        {
            // Arrange
            var newRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };

            // Act
            var result = await _repository.AddExchangeRateAsync(newRate);

            // Assert
            Assert.True(result);
            Assert.Contains(newRate, _context.ExchangeRates);
        }

        [Fact]
        public async Task AddExchangeRateAsync_ReturnsFalse_OnError()
        {
            // Arrange
            var newRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };

            // Simulate exception by disposing the context
            await _context.DisposeAsync();

            // Act
            var result = await _repository.AddExchangeRateAsync(newRate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateExchangeRateAsync_UpdatesRateInDatabase()
        {
            // Arrange
            var existingRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            await _context.ExchangeRates.AddAsync(existingRate);
            await _context.SaveChangesAsync();
            existingRate.Bid = 0.90M; // Updated bid value

            // Act
            var result = await _repository.UpdateExchangeRateAsync(existingRate);

            // Assert
            Assert.True(result);
            var updatedRate = await _context.ExchangeRates.FindAsync(existingRate.Id);
            Assert.Equal(0.90M, updatedRate.Bid);
        }

        [Fact]
        public async Task UpdateExchangeRateAsync_ReturnsFalse_OnError()
        {
            // Arrange
            var existingRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };

            // Simulate exception by disposing the context
            await _context.DisposeAsync();

            // Act
            var result = await _repository.UpdateExchangeRateAsync(existingRate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteExchangeRateAsync_DeletesRateFromDatabase()
        {
            // Arrange
            var rateToDelete = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR" };
            await _context.ExchangeRates.AddAsync(rateToDelete);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteExchangeRateAsync(rateToDelete);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(rateToDelete, _context.ExchangeRates);
        }

        [Fact]
        public async Task DeleteExchangeRateAsync_ReturnsFalse_OnError()
        {
            // Arrange
            var rateToDelete = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR" };

            // Simulate exception by disposing the context
            await _context.DisposeAsync();

            // Act
            var result = await _repository.DeleteExchangeRateAsync(rateToDelete);

            // Assert
            Assert.False(result);
        }
    }
}
