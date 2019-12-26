using System.Collections.Generic;

namespace CarsMatter.Infrastructure.Models
{
    public class CarModel
    {
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public List<Model> Models { get; set; }
    }
}
