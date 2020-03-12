using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FE.Domain.Facades;
using FE.Domain.Facades.Models.Funda;
using Moq;
using Xunit;

namespace FE.Domain.Tests
{
    public class TopTenServiceTests
    {
        private readonly Mock<IFundaFacade> _facadeMock;
        private readonly ITopTenService _service;

        public TopTenServiceTests()
        {
            _facadeMock = new Mock<IFundaFacade>();
            _service = new TopTenService(_facadeMock.Object);
        }

        [Theory]
        [MemberData(nameof(TopTenScenarios))]
        public async Task CalculateTopTenLive_WithResultsFromFacade_ReturnsAccurateTopTen(
            AanbodPage[] pages,
            string[] leaderboardNames,
            int[] leaderboardScores)
        {
            // Arrange
            var pageQueue = new Queue<AanbodPage>(pages);
            // By dequeueing, this will fail hard if it tries getting more pages than it should
            _facadeMock.Setup(f => f.GetAanbod(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => pageQueue.Dequeue());

            // Act
            var lastResult = await _service.CalculateTopTenLive().LastAsync();

            // Assert
            for (var i = 0; i < lastResult.Leaderboard.Length; i++)
            {
                Assert.Equal(leaderboardNames[i], lastResult.Leaderboard[i].AgentName);
                Assert.Equal(leaderboardScores[i], lastResult.Leaderboard[i].NumberOfAds);
            }
        }

        public static IEnumerable<object[]> TopTenScenarios =>
            new List<object[]>
            {
                new object[]
                {
                    // Pages
                    new[]
                    {
                        // Page 1
                        new AanbodPage
                        {
                            TotalNumberOfAds = 5,
                            Paging = new PagingMetadata
                            {
                                CurrentPage = 1,
                                NumberOfPages = 2,
                                NextUrl = "/next",
                                PrevUrl = null,
                            },
                            Ads = new[]
                            {
                                new Aanbod
                                {
                                    AgentId = 1,
                                    AgentName = "Test Testy",
                                },
                                new Aanbod
                                {
                                    AgentId = 1,
                                    AgentName = "Test Testy",
                                },
                                new Aanbod
                                {
                                    AgentId = 2,
                                    AgentName = "Spongebob Squarepants",
                                },
                            },
                        },
                        // Page 2
                        new AanbodPage
                        {
                            TotalNumberOfAds = 5,
                            Paging = new PagingMetadata
                            {
                                CurrentPage = 2,
                                NumberOfPages = 2,
                                NextUrl = null,
                                PrevUrl = "/prev",
                            },
                            Ads = new[]
                            {
                                new Aanbod
                                {
                                    AgentId = 1,
                                    AgentName = "Test Testy",
                                },
                                new Aanbod
                                {
                                    AgentId = 1,
                                    AgentName = "Test Testy",
                                },
                            },
                        },
                    },
                    // Expected Leaderboard Names
                    new []{ "Test Testy", "Spongebob Squarepants" },
                    // Expected Leaderboard Scores
                    new []{ 4, 1 },
                },
            };
    }
}
