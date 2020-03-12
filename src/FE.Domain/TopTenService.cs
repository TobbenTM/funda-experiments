using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using FE.Domain.Facades;
using FE.Domain.Facades.Models.Funda;
using FE.Domain.Models;

namespace FE.Domain
{
    public class TopTenService : ITopTenService
    {
        private readonly IFundaFacade _fundaFacade;

        public TopTenService(IFundaFacade fundaFacade)
        {
            _fundaFacade = fundaFacade;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TopTen> CalculateTopTenLive([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            const string query = "/amsterdam/tuin/";
            var currentPage = 1;
            var leaderboard = new Dictionary<(long agentId, string agentName), int>();
            var currentState = new TopTen();

            AanbodPage page = null;
            do
            {
                page = await _fundaFacade.GetAanbod(query, currentPage, cancellationToken: cancellationToken);
                currentState.TotalAdsToCalculate = page.TotalNumberOfAds;

                foreach (var ad in page.Ads)
                {
                    if (!leaderboard.ContainsKey((ad.AgentId, ad.AgentName)))
                    {
                        leaderboard.Add((ad.AgentId, ad.AgentName), 1);
                    }
                    else
                    {
                        leaderboard[(ad.AgentId, ad.AgentName)] += 1;
                    }
                    currentState.TotalAdsCalculated += 1;
                }

                currentState.Leaderboard = leaderboard
                    .OrderByDescending(kv => kv.Value)
                    .Take(10)
                    .Select(kv => new Ranking
                    {
                        AgentId = kv.Key.agentId,
                        AgentName = kv.Key.agentName,
                        NumberOfAds = kv.Value,
                    })
                    .ToArray();

                yield return currentState;

                currentPage += 1;
            } while (page.Paging.HasMore);
        }
    }
}
