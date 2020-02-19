using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class Car
    {
        public string Id { get; set; }

        public string CarName { get; set; }

        [JsonIgnore]
        public string CarImagePath { get; set; }


        public string Base64CarImage { get; set; }

        public int LowPrice { get; set; }

        public int HighPrice { get; set; }

        public string ManufactureStartDate { get; set; }

        public string ManufactureEndDate { get; set; }

        public string AvitoUri { get; set; }
        
        public string BodyType { get; set; }

        public string BrandModelId { get; set; }

        [ForeignKey("BrandModelId")]
        public BrandModel BrandModel { get; set; }
    }
}
