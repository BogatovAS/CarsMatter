using Newtonsoft.Json;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class User
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}
