using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class RefillNote : TableEntity
    {
        public string Id { get; set; }

        public string Location { get; set; }

        public float Petrol { get; set; }

        public int Odo { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public RefillNote(string id)
        {
            this.Id = id;
            this.PartitionKey = id;
            this.RowKey = id;
        }

        public RefillNote()
        {
        }

    }
}
