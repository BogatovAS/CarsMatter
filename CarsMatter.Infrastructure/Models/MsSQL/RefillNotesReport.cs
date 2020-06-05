namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class RefillNotesReport
    {
        public float TotalCost { get; set; }

        public float CostPerDay { get; set; }

        public float CostPerKm { get; set; }

        public float TotalVolume { get; set; }

        public float AverageVolume { get; set; }
    }
}
