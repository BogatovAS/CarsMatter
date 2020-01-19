using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace CarsMatter.Infrastructure.Repository
{
    public class CarsRepository : ICarsRepository
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<CarsRepository> logger;

        public CarsRepository(CarsMatterDbContext dbContext, ILogger<CarsRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<Car>> GetAllCars(int brandModelId)
        {
            return await Task.Run(() => this.dbContext.Cars.Where(car => car.BrandModelId == brandModelId).ToList());
        }

        public async Task<Car> AddCar(Car car)
        {
            var createdCar = this.dbContext.Add(car);
            await this.SaveChanges();
            return createdCar.Entity;
        }

        public async Task<bool> DeleteCar(int carId)
        {
            Car car = this.dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            this.dbContext.Cars.Remove(car);
            return await this.SaveChanges();
        }

        public async Task<Car> UpdateCar(Car car)
        {
            EntityEntry<Car> updatedCar;
            Car existingCar = this.dbContext.Cars.FirstOrDefault(c => c.Id == car.Id);

            if(existingCar != null)
            {
               updatedCar = this.dbContext.Cars.Update(existingCar);
            }
            else
            {
                updatedCar = this.dbContext.Cars.Add(car);
            }
            await this.SaveChanges();
            return updatedCar.Entity;
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
    }
}
