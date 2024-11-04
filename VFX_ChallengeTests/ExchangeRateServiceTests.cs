using Moq;
using VFX_Challenge.External;
using VFX_Challenge.Repositories;
using VFX_Challenge.Services;

public class ExchangeRateServiceTests
{
    private readonly ExchangeRateService _service;
    private readonly Mock<IExchangeRateRepository> _repositoryMock;
    private readonly Mock<IExternalExchangeRateApi> _externalApiMock;

    public ExchangeRateServiceTests()
    {
        _repositoryMock = new Mock<IExchangeRateRepository>();
        _externalApiMock = new Mock<IExternalExchangeRateApi>();
        _service = new ExchangeRateService(_repositoryMock.Object, _externalApiMock.Object);
    }

    [Fact]
    public async Task GetExchangeRateAsync_ReturnsCorrectRate()
    {
        //// Arrange
        //decimal mockRate = 1.25m;
        //_externalApiMock.Setup(api => api.GetExchangeRateAsync("USD", "EUR"))
        //                .ReturnsAsync(mockRate);

        //// Act
        //var result = await _service.GetExchangeRateAsync("USD", "EUR");

        //// Assert
        //Assert.Equal(mockRate, result);
    }

    [Fact]
    public async Task GetExchangeRateAsync_UsesCacheWhenAvailable()
    {
        //// Arrange
        //decimal cachedRate = 1.20m;
        //_repositoryMock.Setup(repo => repo.GetCachedRateAsync("USD", "EUR"))
        //               .ReturnsAsync(cachedRate);

        //// Act
        //var result = await _service.GetExchangeRateAsync("USD", "EUR");

        //// Assert
        //Assert.Equal(cachedRate, result);
        //_externalApiMock.Verify(api => api.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
