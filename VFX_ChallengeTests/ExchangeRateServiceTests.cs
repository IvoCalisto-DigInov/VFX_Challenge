using Microsoft.Extensions.Logging;
using Moq;
using VFX_Challenge.External;
using VFX_Challenge.Models;
using VFX_Challenge.Repositories;
using VFX_Challenge.Services;
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
            var expectedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync(expectedRate);

            var result = await _service.GetExchangeRateAsync("USD", "EUR");

            Assert.Equal(expectedRate, result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            //_externalApiMock.Verify(api => api.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetExchangeRateAsync_FetchesFromExternalApi_WhenNotInDatabase()
        {
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);
            var expectedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            //_externalApiMock.Setup(api => api.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync(expectedRate);

            var result = await _service.GetExchangeRateAsync("USD", "EUR");

            Assert.Equal(expectedRate, result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            //_externalApiMock.Verify(api => api.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.AddExchangeRateAsync(expectedRate), Times.Once);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsNull_WhenNotFoundInDatabaseOrApi()
        {
            _repositoryMock.Setup(repo => repo.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);
            //_externalApiMock.Setup(api => api.GetExchangeRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);

            var result = await _service.GetExchangeRateAsync("USD", "EUR");

            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            //_externalApiMock.Verify(api => api.GetExchangeRateAsync("USD", "EUR"), Times.Once);
            _repositoryMock.Verify(repo => repo.AddExchangeRateAsync(It.IsAny<ExchangeRate>()), Times.Never);
        }
    }
}