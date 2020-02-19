using System;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class RefillNote
    {
        public string Id { get; set; }

        public string Location { get; set; }

        public float Petrol { get; set; }

        public int Odo { get; set; }

        public float Price { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

    }
}
