using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class ConsumablesNote : TableEntity
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string KindOfService { get; set; }

        public float Price { get; set; }

        public int Odo { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public ConsumablesNote()
        {
        }

        public ConsumablesNote(string id)
        {
            this.Id = id;
            this.PartitionKey = id;
            this.RowKey = id;
        }
    }
}
