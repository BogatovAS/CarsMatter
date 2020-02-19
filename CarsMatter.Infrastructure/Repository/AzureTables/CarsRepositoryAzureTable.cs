using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;
using Microsoft.Extensions.Caching.Memory;

namespace CarsMatter.Infrastructure.Repository.AzureTables
{
    public class CarsRepositoryAzureTable : ICarsRepository<Car>
    {
        private readonly IAzureTable<Car> carTable;

        private readonly HttpClient httpClient;

        private readonly IMemoryCache memoryCache;

        private readonly MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        public CarsRepositoryAzureTable(
            IAzureTable<Car> carTable, 
            IMemoryCache memoryCache, 
            IHttpClientFactory httpClientFactory)
        {
            this.carTable = carTable;
            this.memoryCache = memoryCache;
            this.httpClient = httpClientFactory.CreateClient("avtomarket");
        }

        public async Task<List<Car>> GetAllCars(string brandModelId)
        {
            List<Car> cars;
            if (this.memoryCache.TryGetValue($"brand.model.{brandModelId}.cars", out cars))
            {
                return cars;
            }

            cars = await this.carTable.GetList(nameof(Car.BrandModelId), brandModelId);
            this.memoryCache.Set($"brand.model.{brandModelId}.cars", cars, this.memoryCacheEntryOptions);

            return cars;
        }

        public async Task<List<Car>> GetAllCars()
        {
            List<Car> cars;
            if (this.memoryCache.TryGetValue("all.cars", out cars))
            {
                return cars;
            }

            var allCars = await this.carTable.GetList();
            this.memoryCache.Set("all.cars", allCars, this.memoryCacheEntryOptions);

            return allCars;
        }

        public async Task<Car> GetFullCarInfo(string carId)
        {
            var car = await this.carTable.GetItem(carId, carId);
            car.Base64CarImage = await GetImageForModel(car.CarImagePath);

            return car;
        }

        public Task AddCar(Car car)
        {
            return this.carTable.Insert(car);
        }

        public Task DeleteCar(string carId)
        {
            return this.carTable.Delete(carId, carId);
        }

        public Task UpdateCar(Car car)
        {
            return this.carTable.Update(car);
        }

        public async Task<string> GetImageForModel(string modelImageHttpPath)
        {
            string carImageBase64;

            if (this.memoryCache.TryGetValue($"car.{modelImageHttpPath}.image", out carImageBase64))
            {
                return carImageBase64;
            }

            HttpResponseMessage carImageResponse = await this.httpClient.GetAsync(modelImageHttpPath);
            carImageBase64 = Convert.ToBase64String(await carImageResponse.Content.ReadAsByteArrayAsync());
            this.memoryCache.Set($"car.{modelImageHttpPath}.image", carImageBase64);

            return carImageBase64;
        }
    }
}
