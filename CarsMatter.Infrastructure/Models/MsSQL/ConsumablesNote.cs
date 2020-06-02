using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class ConsumablesNote
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public float Price { get; set; }

        public int Odo { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }

        public string MyCarId { get; set; }

        [JsonIgnore]
        public MyCar MyCar { get; set; }

        public string KindOfServiceId { get; set; }

        public KindOfService KindOfService { get; set; }
    }
}
