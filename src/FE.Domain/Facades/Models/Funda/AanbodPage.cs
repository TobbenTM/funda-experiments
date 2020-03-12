using Newtonsoft.Json;

namespace FE.Domain.Facades.Models.Funda
{
    public class AanbodPage
    {
        [JsonProperty("Objects")]
        public Aanbod[] Ads { get; set; }

        [JsonProperty("Paging")]
        public PagingMetadata Paging { get; set; }

        [JsonProperty("TotaalAantalObjecten")]
        public long TotalNumberOfAds { get; set; }
    }
}
