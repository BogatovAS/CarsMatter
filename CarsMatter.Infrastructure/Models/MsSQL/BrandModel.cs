using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class BrandModel
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public string BrandId { get; set; }

        [JsonIgnore]
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
