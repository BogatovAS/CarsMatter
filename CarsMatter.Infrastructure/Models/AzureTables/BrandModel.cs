using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class BrandModel : TableEntity
    {
        public string Id { get; set; }

        [JsonIgnore]
        [IgnoreProperty]
        public string HttpPath { get; set; }

        public string ModelName { get; set; }

        public string BrandId { get; set; }

        public BrandModel(string id)
        {
            this.Id = id;
            this.PartitionKey = id;
            this.RowKey = id;
        }

        public BrandModel()
        {
        }
    }
}
