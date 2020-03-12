using Newtonsoft.Json;

namespace FE.Domain.Facades.Models.Funda
{
    public class Aanbod
    {
        [JsonProperty("MakelaarId")]
        public long AgentId { get; set; }

        [JsonProperty("MakelaarNaam")]
        public string AgentName { get; set; }
    }
}
