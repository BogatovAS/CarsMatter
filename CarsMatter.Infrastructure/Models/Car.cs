using CarsMatter.Infrastructure.Models.Enums;

namespace CarsMatter.Infrastructure.Models
{
    public class Car
    {
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public string CarImagePath { get; set; }

        public string Base64CarImage { get; set; }

        public decimal LowPrice { get; set; }

        public decimal HighPrice { get; set; }

        public string ManufactureStartDate { get; set; }

        public string ManufactureEndDate { get; set; }

        public string AvitoUri { get; set; }

        public BodyType BodyType { get; set; }
    }
}
