using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using VFX_Challenge.External;
using VFX_Challenge.Models;
using VFX_Challenge.Repositories;
using VFX_Challenge.Services;
using Xunit;

namespace VFX_ChallengeTests
{
    public class ExchangeRateServiceTests
    {
        private readonly Mock<IExchangeRateRepository> _repositoryMock;
        private readonly Mock<IExternalExchangeRateApi> _externalApiMock;
        private readonly Mock<ILogger<ExchangeRateService>> _loggerMock;
        private readonly ExchangeRateService _service;

        public ExchangeRateServiceTests()
        {
            _repositoryMock = new Mock<IExchangeRateRepository>();
            _externalApiMock = new Mock<IExternalExchangeRateApi>();
            _loggerMock = new Mock<ILogger<ExchangeRateService>>();
            _service = new ExchangeRateService(_repositoryMock.Object, _externalApiMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRateFromDatabase()
        {
            // Arrange
            var expectedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync(expectedRate);

            // Act
            var result = await _service.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Equal(expectedRate, result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _externalApiMock.Verify(api => api.FetchExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetExchangeRateAsync_FetchesFromExternalApi_WhenNotInDatabase()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);
            var expectedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            _externalApiMock.Setup(api => api.FetchExchangeRateAsync("USD", "EUR")).ReturnsAsync(expectedRate);

            // Act
            var result = await _service.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Equal(expectedRate, result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _externalApiMock.Verify(api => api.FetchExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.AddExchangeRateAsync(expectedRate), Times.Once);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_WhenNotFoundInDatabaseOrApi()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);
            _externalApiMock.Setup(api => api.FetchExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);

            // Act
            var result = await _service.GetExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _externalApiMock.Verify(api => api.FetchExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.AddExchangeRateAsync(It.IsAny<ExchangeRate>()), Times.Never);
        }

        [Fact]
        public async Task GetAllExchangeRatesAsync_ReturnsAllRates()
        {
            // Arrange
            var rates = new List<ExchangeRate> { new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR" } };
            _repositoryMock.Setup(repo => repo.GetAllExchangeRatesAsync()).ReturnsAsync(rates);

            // Act
            var result = await _service.GetAllExchangeRatesAsync();

            // Assert
            Assert.Equal(rates, result);
            _repositoryMock.Verify(repo => repo.GetAllExchangeRatesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddExchangeRateAsync_ReturnsTrue_WhenRateIsAdded()
        {
            // Arrange
            var newRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            _repositoryMock.Setup(repo => repo.AddExchangeRateAsync(newRate)).ReturnsAsync(true);

            // Act
            var result = await _service.AddExchangeRateAsync(newRate);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.AddExchangeRateAsync(newRate), Times.Once);
        }

        [Fact]
        public async Task UpdateExchangeRateAsync_UpdatesRate_WhenRateExists()
        {
            // Arrange
            var existingRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            var updatedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.90M };
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync(existingRate);
            _repositoryMock.Setup(repo => repo.UpdateExchangeRateAsync(existingRate)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateExchangeRateAsync("USD", "EUR", updatedRate);

            // Assert
            Assert.True(result);
            Assert.Equal(0.90M, existingRate.Bid); // Verify updated value
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateExchangeRateAsync(existingRate), Times.Once);
        }

        [Fact]
        public async Task UpdateExchangeRateAsync_ReturnsFalse_WhenRateDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);

            // Act
            var result = await _service.UpdateExchangeRateAsync("USD", "EUR", new ExchangeRate());

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateExchangeRateAsync(It.IsAny<ExchangeRate>()), Times.Never);
        }

        [Fact]
        public async Task DeleteExchangeRateAsync_DeletesRate_WhenRateExists()
        {
            // Arrange
            var rateToDelete = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR" };
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync(rateToDelete);
            _repositoryMock.Setup(repo => repo.DeleteExchangeRateAsync(rateToDelete)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.DeleteExchangeRateAsync(rateToDelete), Times.Once);
        }

        [Fact]
        public async Task DeleteExchangeRateAsync_ReturnsFalse_WhenRateDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);

            // Act
            var result = await _service.DeleteExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.DeleteExchangeRateAsync(It.IsAny<ExchangeRate>()), Times.Never);
        }
    }
}
