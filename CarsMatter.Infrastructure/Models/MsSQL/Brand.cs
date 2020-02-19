using Newtonsoft.Json;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class Brand
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string HttpPath { get; set; }

        public string BrandName { get; set; }
    }
}
