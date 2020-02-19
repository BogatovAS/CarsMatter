using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Security;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class User : TableEntity
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public User(string id)
        {
            this.Id = id;
            this.RowKey = id;
            this.PartitionKey = id;
        }

        public User()
        {
        }
    }
}
