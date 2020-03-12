using System.ComponentModel.DataAnnotations;

namespace FE.Domain.Configuration
{
    public class FundaConfiguration
    {
        [Required]
        public string ApiKey { get; set; }
    }
}
