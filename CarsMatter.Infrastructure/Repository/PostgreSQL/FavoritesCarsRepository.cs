using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.MsSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Repository
{
    public class FavoriteCarsRepository : IFavoriteCarsRepository<Car>
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<FavoriteCarsRepository> logger;

        public FavoriteCarsRepository(CarsMatterDbContext dbContext, ILogger<FavoriteCarsRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task Add(string userId, string carId)
        {
            FavoriteCar favoriteCarRecord = new FavoriteCar
            {
                CarId = carId,
                UserId = userId
            };

            this.dbContext.FavoriteCars.Add(favoriteCarRecord);
            await this.SaveChanges();
        }

        public async Task Delete(string userId, string carId)
        {
            FavoriteCar favoriteCarRecord = this.dbContext.FavoriteCars.FirstOrDefault(fc => fc.UserId == userId && fc.CarId == carId);
            this.dbContext.FavoriteCars.Remove(favoriteCarRecord);

            await this.SaveChanges();
        }

        public async Task<List<Car>> GetFavoriteCars(string userId) 
        {
            var favoriteCars = await Task.Run(() => this.dbContext.FavoriteCars.Where(fc => fc.UserId == userId).Include(fc => fc.Car).ToList());
            return favoriteCars.Select(fc => fc.Car).ToList();
        }

        public async Task<bool> IsFavoriteCar(string userId, string carId)
        {
            FavoriteCar result = await this.dbContext.FavoriteCars.FirstOrDefaultAsync(fc => fc.UserId == userId && fc.CarId == carId);

            return result != null;
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
