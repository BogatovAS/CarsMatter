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
            User user = await this.dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            UserCar favoriteCar = new UserCar
            {
                CarId = carId,
                UserId = userId
            };

            user.UsersCars.Add(favoriteCar);

            await this.SaveChanges();
        }

        public async Task Delete(string userId, string carId)
        {
            User user = await this.dbContext.Users
                .Include(user => user.UsersCars)
                .FirstOrDefaultAsync(user => user.Id == userId);

            var favoriteCar = user.UsersCars.FirstOrDefault(userCar => userCar.UserId == userId && userCar.CarId == carId);

            user.UsersCars.Remove(favoriteCar);

            await this.SaveChanges();
        }

        public async Task<List<Car>> GetFavoriteCars(string userId) 
        {
            User user = await this.dbContext.Users
                .Include(user => user.UsersCars)
                .ThenInclude(usersCars => usersCars.Car)
                .FirstOrDefaultAsync(user => user.Id == userId);

            var favoriteCars = user.UsersCars.Where(userCar => userCar.UserId == userId).Select(userCar => userCar.Car);

            return favoriteCars.ToList();
        }

        public async Task<bool> IsFavoriteCar(string userId, string carId)
        {
            User user = await this.dbContext.Users
                .Include(user => user.UsersCars)
                .ThenInclude(usersCars => usersCars.Car)
                .FirstOrDefaultAsync(user => user.Id == userId);

            var favoriteCar = user.UsersCars.FirstOrDefault(userCar => userCar.UserId == userId && userCar.CarId == carId);

            return favoriteCar != null;
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
