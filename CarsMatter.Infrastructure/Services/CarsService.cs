using CarsMatter.Infrastructure.Helpers;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Services
{
    public class CarsService : ICarsService
    {
        private readonly HttpClient httpClient;

        private readonly IMemoryCache memoryCache;

        private readonly ILogger<CarsService> logger;

        private readonly MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        public CarsService(HttpClient httpClient, IMemoryCache memoryCache, ILogger<CarsService> logger)
        {
            this.httpClient = httpClient;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            List<Brand> brands = new List<Brand>();
            if (this.memoryCache.TryGetValue("car.brands", out brands))
            {
                return brands;
            }

            HttpResponseMessage response = await httpClient.GetAsync("catalog/");
            string htmlDocument = await response.Content.ReadAsStringAsync();

            try
            {
                brands = await CarsHtmlParser.ParseBrands(htmlDocument);
                this.memoryCache.Set("car.brands", brands, this.memoryCacheEntryOptions);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"An error occured while trying to parse brands. Message: {e.Message}");
            }

            return brands;
        }

        public async Task<List<BrandModel>> GetAllBrandModels(string brandHttpPath)
        {
            List<BrandModel> brandModels = new List<BrandModel>();
            if (this.memoryCache.TryGetValue($"car.brand.{brandHttpPath}.models", out brandModels))
            {
                return brandModels;
            }

            HttpResponseMessage response = await httpClient.GetAsync(brandHttpPath);
            string htmlDocument = await response.Content.ReadAsStringAsync();

            try
            {
                brandModels = await CarsHtmlParser.ParseBrandModels(htmlDocument);
                this.memoryCache.Set($"car.brand.{brandHttpPath}.models", brandModels, this.memoryCacheEntryOptions);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"An error occured while trying to parse models for brand with http path: {brandHttpPath}. Message: {e.Message}");
            }

            return brandModels;
        }

        public async Task<List<Car>> GetAllCarsForModel(string carModelHttpPath)
        {
            List<Car> cars = new List<Car>();
            if (this.memoryCache.TryGetValue($"car.model.{carModelHttpPath}.cars", out cars))
            {
                return cars;
            }

            HttpResponseMessage response = await httpClient.GetAsync(carModelHttpPath);
            string htmlDocument = await response.Content.ReadAsStringAsync();

            try
            {
                cars = await CarsHtmlParser.ParseCarsForModel(htmlDocument);

                for(int i=0; i < cars.Count; i++)
                {
                    cars[i].Base64CarImage = await GetImageForModel(cars[i].CarImagePath);
                }

                this.memoryCache.Set($"car.model.{carModelHttpPath}.cars", cars, this.memoryCacheEntryOptions);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"An error occured while trying to parse cars for model with http path: {carModelHttpPath}. Message: {e.Message}");
            }

            return cars;
        }

        private async Task<string> GetImageForModel(string modelImageHttpPath)
        {
            HttpResponseMessage carImageResponse = await httpClient.GetAsync(modelImageHttpPath);
            string carImageBase64 = Convert.ToBase64String(await carImageResponse.Content.ReadAsByteArrayAsync());

            return carImageBase64;
        }
    }
}
