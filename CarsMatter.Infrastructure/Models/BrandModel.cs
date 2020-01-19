using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models
{
    public class BrandModel
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
