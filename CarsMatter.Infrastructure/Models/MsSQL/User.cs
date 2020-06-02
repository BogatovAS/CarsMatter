using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class User
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        [JsonIgnore]
        public ICollection<UserCar> UsersCars { get; set; }

        public ICollection<MyCar> MyCars { get; set; }

        public string MySelectedCarId { get; set; }

        public User()
        {
            this.UsersCars = new List<UserCar>();
            this.MyCars = new List<MyCar>();
        }
    }
}
