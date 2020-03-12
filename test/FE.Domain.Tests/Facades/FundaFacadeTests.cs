using System;
using System.Net.Http;
using System.Threading.Tasks;
using FE.Domain.Configuration;
using FE.Domain.Facades;
using Xunit;

namespace FE.Domain.Tests.Facades
{
    /// <summary>
    /// These are pretty much integration tests, but could be
    /// turned into more local tests by mocking the HttpClient
    /// </summary>
    public class FundaFacadeTests
    {
        private readonly IFundaFacade _facade;

        public FundaFacadeTests()
        {
            var client = new HttpClient();

            // Sorta ugly way to create configuration, doing it this way in the interest of time
            var configuration = new FundaConfiguration
            {
                ApiKey = Environment.GetEnvironmentVariable("ASPNETCORE__Funda__ApiKey"),
            };

            _facade = new FundaFacade(client, configuration);
        }

        [Fact]
        public async Task GetAanbod_WithQuery_ReturnsAds()
        {
            // Arrange
            var query = "/amsterdam/tuin/";

            // Act
            var result = await _facade.GetAanbod(query, 1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Ads);
            Assert.True(result.Ads.Length > 0);
        }
    }
}
