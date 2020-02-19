using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class FavoriteCar
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }
    }
}
