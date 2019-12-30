using CarsMatter.Infrastructure.Models.Enums;

namespace CarsMatter.Infrastructure.Models
{
    public class Model
    {
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public string CarImagePath { get; set; }

        public decimal LowPrice { get; set; }

        public decimal HighPrice { get; set; }

        public BodyType BodyType { get; set; }
    }
}
