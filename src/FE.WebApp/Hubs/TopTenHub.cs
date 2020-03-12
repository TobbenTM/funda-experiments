using System.Threading.Tasks;
using FE.Domain;
using Microsoft.AspNetCore.SignalR;

namespace FE.WebApp.Hubs
{
    public class TopTenHub : Hub<ITopTenHubClient>
    {
        private readonly ITopTenService _service;

        public TopTenHub(ITopTenService service)
        {
            _service = service;
        }

        /// <summary>
        /// Will start calculating leaderboard, and invoke LeaderboardUpdated
        /// each time new results are available.
        /// </summary>
        public async Task StartCalculating()
        {
            var stream = _service.CalculateTopTenLive();
            await foreach (var topTen in stream)
            {
                await Clients.Caller.LeaderboardUpdated(topTen);
            }
        }
    }
}
