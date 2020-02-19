using Microsoft.WindowsAzure.Storage.Table;

namespace CarsMatter.Infrastructure.Models.AzureTables
{
    public class FavoriteCar : TableEntity
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string CarId { get; set; }

        [IgnoreProperty]
        public Car Car { get; set; }

        public FavoriteCar(string id, string userId, string carId)
        {
            this.Id = id;
            this.PartitionKey = userId;
            this.RowKey = carId;

            this.UserId = userId;
            this.CarId = carId;
        }

        public FavoriteCar()
        {
        }
    }
}
