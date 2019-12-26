using System.Collections.Generic;

namespace CarsMatter.Infrastructure.Models
{
    public class Model
    {
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public string CarImage { get; set; }

        public List<Car> Cars { get; set; }
    }
}
