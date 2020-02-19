using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class Brand : TableEntity
    {
        public string Id { get; set; }

        [JsonIgnore]
        [IgnoreProperty]
        public string HttpPath { get; set; }

        public string BrandName { get; set; }

        public Brand(string id)
        {
            this.Id = id;
            this.PartitionKey = id;
            this.RowKey = id;
        }

        public Brand()
        {
        }
    }
}
