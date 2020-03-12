using Newtonsoft.Json;

namespace FE.Domain.Facades.Models.Funda
{
    public class PagingMetadata
    {
        [JsonProperty("AantalPaginas")]
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Not sure CurrentPage is a good translation
        /// </summary>
        [JsonProperty("HuidigePagina")]
        public int CurrentPage { get; set; }

        [JsonProperty("VolgendeUrl")]
        public string NextUrl { get; set; }

        [JsonProperty("VorigeUrl")]
        public string PrevUrl { get; set; }

        public bool HasMore => NextUrl != null;
    }
}
