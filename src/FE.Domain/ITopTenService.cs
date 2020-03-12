using System.Collections.Generic;
using System.Threading;
using FE.Domain.Models;

namespace FE.Domain
{
    public interface ITopTenService
    {
        /// <summary>
        /// This will continuously yield results until it has calculated
        /// all results (pages) available for the query. Might be a bit
        /// hacky solution to emulate a stream, but lets us very easily
        /// react to new calculations using native foreach or linq constructs
        /// </summary>
        IAsyncEnumerable<TopTen> CalculateTopTenLive(CancellationToken cancellationToken = default);
    }
}
