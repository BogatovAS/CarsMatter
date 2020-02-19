using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Repository.AzureTables
{
    public class FavoriteCarsRepositoryAzureTable : IFavoriteCarsRepository<FavoriteCar>
    {
        private readonly IAzureTable<FavoriteCar> favoriteCarsTable;
        private readonly IAzureTable<Car> carsTable;

        public FavoriteCarsRepositoryAzureTable(
            IAzureTable<FavoriteCar> favoriteCarsTable,
            IAzureTable<Car> carsTable)
        {
            this.favoriteCarsTable = favoriteCarsTable;
            this.carsTable = carsTable;
        }

        public Task Add(string userId, string carId)
        {
            FavoriteCar favoriteCar = new FavoriteCar(Guid.NewGuid().ToString(), userId, carId);

            return this.favoriteCarsTable.Insert(favoriteCar);
        }

        public Task Delete(string userId, string carId)
        {
            return this.favoriteCarsTable.Delete(userId, carId);
        }

        public async Task<List<FavoriteCar>> GetFavoriteCars(string userId)
        {
            List<FavoriteCar> favoriteCars = await this.favoriteCarsTable.GetList(nameof(FavoriteCar.UserId), userId);

            foreach (var favoriteCar in favoriteCars)
            {
                favoriteCar.Car = await this.carsTable.GetItem(favoriteCar.CarId, favoriteCar.CarId);
            }

            return favoriteCars;
        }

        public async Task<bool> IsFavoriteCar(string userId, string carId)
        {
            FavoriteCar existingCar = await this.favoriteCarsTable.GetItem(userId, carId);

            return existingCar != null;
        }
    }
}
