using System;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class ConsumablesNote
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string KindOfService { get; set; }

        public float Price { get; set; }

        public int Odo { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }
    }
}
