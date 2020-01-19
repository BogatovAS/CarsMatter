using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Repository
{
    public class FavoriteCarsRepository : IFavoriteCarsRepository
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<FavoriteCarsRepository> logger;

        public FavoriteCarsRepository(CarsMatterDbContext dbContext, ILogger<FavoriteCarsRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<bool> Add(int userId, int carId)
        {
            FavoriteCar favoriteCarRecord = new FavoriteCar
            {
                CarId = carId,
                UserId = userId
            };

            this.dbContext.FavoriteCars.Add(favoriteCarRecord);
            return await this.SaveChanges();
        }

        public async Task<bool> Delete(int favoriteRecordId)
        {
            FavoriteCar favoriteCarRecord = this.dbContext.FavoriteCars.FirstOrDefault(fc => fc.Id == favoriteRecordId);
            this.dbContext.FavoriteCars.Remove(favoriteCarRecord);

            return await this.SaveChanges();
        }

        public async Task<List<Car>> GetFavoriteCars(int userId) 
        {
            var favoriteCars = await Task.Run(() => this.dbContext.FavoriteCars.Where(fc => fc.UserId == userId).Include(fc => fc.Car).ToList());
            return favoriteCars.Select(fc => fc.Car).ToList();
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
