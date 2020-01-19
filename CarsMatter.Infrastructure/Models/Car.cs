using CarsMatter.Infrastructure.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models
{
    public class Car
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        [JsonIgnore]
        public string CarImagePath { get; set; }

        public string Base64CarImage { get; set; }

        public decimal LowPrice { get; set; }

        public decimal HighPrice { get; set; }

        public string ManufactureStartDate { get; set; }

        public string ManufactureEndDate { get; set; }

        public string AvitoUri { get; set; }
        
        public BodyType BodyType { get; set; }

        public int BrandModelId { get; set; }

        [ForeignKey("BrandModelId")]
        public BrandModel BrandModel { get; set; }
    }
}
