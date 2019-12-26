using CarsMatter.Infrastructure.Models.Enums;

namespace CarsMatter.Infrastructure.Models
{
    public class Car
    {
        public string Name { get; set; }

        public string AvitoUri { get; set; }

        public Сharacteristics Characteristics { get; set; }

        public class Сharacteristics
        {
            public string ManufactureStartDate { get; set; }

            public string ManufactureEndDate { get; set; }

            public decimal LowPrice { get; set; }

            public decimal HighPrice { get; set; }

            public string Transmission { get; set; }

            public BodyType BodyType { get; set; }
        }
    }

}
