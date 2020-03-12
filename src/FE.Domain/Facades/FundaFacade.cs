using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FE.Domain.Configuration;
using FE.Domain.Facades.Models.Funda;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;

namespace FE.Domain.Facades
{
    /// <summary>
    /// Facade against the Funda API, where we can do retry logic,
    /// incremental timeouts and circuit breaking
    /// </summary>
    public class FundaFacade : IFundaFacade
    {
        // Hardcoded for now, could move to config
        private const string BaseUrl = "http://partnerapi.funda.nl";

        private readonly HttpClient _httpClient;
        private readonly string _anbodPath;

        public FundaFacade(HttpClient httpClient, FundaConfiguration configuration)
        {
            _httpClient = httpClient;

            // Only doing this here because I'm not creating a custom HttpClient
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _anbodPath = $"/feeds/Aanbod.svc/json/{configuration.ApiKey}";
        }

        public async Task<AanbodPage> GetAanbod(string query, int page, int pageSize = 25, CancellationToken cancellationToken = default)
        {
            // Polly policy to retry until successful, or fail hard
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromSeconds(60),
                });

            // TODO: Add cancellationToken to policy
            var result = await retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{_anbodPath}?type=koop&zo={query}&page={page}&pageSize={pageSize}", cancellationToken));

            result.EnsureSuccessStatusCode();

            var model = JsonConvert.DeserializeObject<AanbodPage>(await result.Content.ReadAsStringAsync());

            return model;
        }
    }
}
