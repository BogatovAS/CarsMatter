using System;

namespace CarsMatter.Infrastructure.Models.Journal
{
    public class RefillNote
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public float Petrol { get; set; }

        public int Odo { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

    }
}
