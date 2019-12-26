using System.Collections.Generic;

namespace CarsMatter.Infrastructure.Models
{
    public class Brand
    {
        public string HttpPath { get; set; }

        public string BrandName { get; set; }

        public List<CarModel> Models { get; set;}
    }
}
