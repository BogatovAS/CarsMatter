using Newtonsoft.Json;
using System.Collections.Generic;

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

        public User()
        {
            this.UsersCars = new List<UserCar>();
        }
    }
}
