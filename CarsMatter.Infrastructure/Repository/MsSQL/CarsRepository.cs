using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.MsSQL;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CarsMatter.Infrastructure.Repository
{
    public class CarsRepository : ICarsRepository<Car>
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly HttpClient httpClient;

        private readonly IMemoryCache memoryCache;

        private readonly MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        private readonly ILogger<CarsRepository> logger;

        public CarsRepository(CarsMatterDbContext dbContext, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, ILogger<CarsRepository> logger)
        {
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
            this.httpClient = httpClientFactory.CreateClient("avtomarket");
            this.logger = logger;
        }

        public async Task<List<Car>> GetAllCars(string brandModelId)
        {
            List<Car> cars;
            if (this.memoryCache.TryGetValue($"brand.model.{brandModelId}.cars", out cars))
            {
                return cars;
            }

            cars = await Task.Run(() => this.dbContext.Cars
            .Where(car => car.BrandModelId == brandModelId)
            .OrderBy(car => car.CarName)
            .ToList());

            this.memoryCache.Set($"brand.model.{brandModelId}.cars", cars, this.memoryCacheEntryOptions);

            return cars;
        }

        public async Task<List<Car>> GetAllCars()
        {
            List<Car> cars;
            if (this.memoryCache.TryGetValue($"all.cars", out cars))
            {
                return cars;
            }

            cars = await Task.Run(() => this.dbContext.Cars.OrderBy(car => car.CarName).ToList());

            this.memoryCache.Set("all.cars", cars, this.memoryCacheEntryOptions);

            return cars;
        }

        public async Task<Car> GetFullCarInfo(string carId)
        {
            return await Task.Run(() => this.dbContext.Cars.First(car => car.Id == carId));
        }

        public async Task AddCar(Car car)
        {
            this.dbContext.Add(car);
            await this.SaveChanges();
        }

        public async Task DeleteCar(string carId)
        {
            Car car = this.dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            this.dbContext.Cars.Remove(car);
            await this.SaveChanges();
        }

        public async Task UpdateCar(Car car)
        {
            EntityEntry<Car> updatedCar;
            Car existingCar = this.dbContext.Cars.FirstOrDefault(c => c.HttpPath == car.HttpPath);

            if(existingCar != null)
            {
               updatedCar = this.dbContext.Cars.Update(existingCar);
            }
            else
            {
                updatedCar = this.dbContext.Cars.Add(car);
            }
            await this.SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                throw;
            }
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
