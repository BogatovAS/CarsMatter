﻿using CarsMatter.Infrastructure.Helpers;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.MsSQL;
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

        private readonly ILogger<CarsService> logger;

        public CarsService(IHttpClientFactory httpClientFactory, ILogger<CarsService> logger)
        {
            this.httpClient = httpClientFactory.CreateClient("avtomarket");
            this.logger = logger;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            List<Brand> brands = new List<Brand>();

            HttpResponseMessage response = await this.httpClient.GetAsync("catalog/");
            string htmlDocument = await response.Content.ReadAsStringAsync();

            try
            {
                brands = await CarsHtmlParser.ParseBrands(htmlDocument);
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

            HttpResponseMessage response = await this.httpClient.GetAsync(brandHttpPath);
            string htmlDocument = await response.Content.ReadAsStringAsync();

            try
            {
                brandModels = await CarsHtmlParser.ParseBrandModels(htmlDocument);
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

            HttpResponseMessage response = await this.httpClient.GetAsync(carModelHttpPath);
            string htmlDocument = await response.Content.ReadAsStringAsync();

            try
            {
                cars = await CarsHtmlParser.ParseCarsForModel(htmlDocument);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"An error occured while trying to parse cars for model with http path: {carModelHttpPath}. Message: {e.Message}");
            }

            return cars;
        }
    }
}
