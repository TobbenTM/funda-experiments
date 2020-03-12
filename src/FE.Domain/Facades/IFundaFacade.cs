using System.Threading;
using System.Threading.Tasks;
using FE.Domain.Facades.Models.Funda;

namespace FE.Domain.Facades
{
    public interface IFundaFacade
    {
        Task<AanbodPage> GetAanbod(string query, int page, int pageSize = 25, CancellationToken cancellationToken = default);
    }
}
