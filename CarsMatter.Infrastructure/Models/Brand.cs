using Newtonsoft.Json;

namespace CarsMatter.Infrastructure.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string HttpPath { get; set; }

        public string BrandName { get; set; }
    }
}
