using System.Threading.Tasks;
using FE.Domain.Models;

namespace FE.WebApp.Hubs
{
    public interface ITopTenHubClient
    {
        Task LeaderboardUpdated(TopTen topTen);
    }
}
