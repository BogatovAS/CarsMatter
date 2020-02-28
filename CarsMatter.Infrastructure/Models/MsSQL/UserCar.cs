using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class UserCar
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Key, ForeignKey("Car")]
        public string CarId { get; set; }

        public Car Car { get; set; }
    }
}
