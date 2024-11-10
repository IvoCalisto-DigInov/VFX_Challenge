using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VFX_Challenge.Models;
using VFX_Challenge.Repositories;
using Xunit;

namespace VFX_ChallengeTests
{
    public class ExchangeRateRepositoryTests
    {
        private readonly Mock<ExchangeRateDbContext> _contextMock;
        private readonly Mock<ILogger<ExchangeRateRepository>> _loggerMock;
        private readonly ExchangeRateRepository _repository;

        public ExchangeRateRepositoryTests()
        {
            _contextMock = new Mock<ExchangeRateDbContext>();
            _loggerMock = new Mock<ILogger<ExchangeRateRepository>>();
            _repository = new ExchangeRateRepository(_contextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetExchangeRateAsync_ReturnsRateFromDatabase()
        {
            var expectedRate = new ExchangeRate { BaseCurrency = "USD", QuoteCurrency = "EUR", Bid = 0.85M };
            _contextMock.Setup(ctx => ctx.ExchangeRates.FindAsync("USD", "EUR")).ReturnsAsync(expectedRate);

            var result = await _repository.GetExchangeRateAsync("USD", "EUR");

            Assert.Equal(expectedRate, result);
            _contextMock.Verify(ctx => ctx.ExchangeRates.FindAsync("USD", "EUR"), Times.Once);
        }
    }
}
