using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models
{
    public class FavoriteCar
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }
    }
}
