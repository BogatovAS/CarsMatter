using System;

namespace CarsMatter.Infrastructure.Models.Journal
{
    public class ConsumablesNote
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string KindOfService { get; set; }

        public float Price { get; set; }

        public int Odo { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }
    }
}
