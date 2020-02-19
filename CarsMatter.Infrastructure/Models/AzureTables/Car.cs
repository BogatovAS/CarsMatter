using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class Car : TableEntity
    {
        public string Id { get; set; }

        public string CarName { get; set; }

        [JsonIgnore]
        public string CarImagePath { get; set; }

        [IgnoreProperty]
        public string Base64CarImage { get; set; }

        public decimal LowPrice { get; set; }

        public decimal HighPrice { get; set; }

        public string ManufactureStartDate { get; set; }

        public string ManufactureEndDate { get; set; }

        public string AvitoUri { get; set; }
        
        public string BodyType { get; set; }

        public string BrandModelId { get; set; }

        public Car(string id)
        {
            this.Id = id;
            this.PartitionKey = id;
            this.RowKey = id;
        }

        public Car()
        {
        }
    }
}
