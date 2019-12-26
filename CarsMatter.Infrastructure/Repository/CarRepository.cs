namespace CarsMatter.Infrastructure.Repository
{
    using CarsMatter.Infrastructure.Db;
    using CarsMatter.Infrastructure.Interfaces;
    using CarsMatter.Infrastructure.Models;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class CarRepository : ICarRepository
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<CarRepository> logger;

        public CarRepository(CarsMatterDbContext dbContext, ILogger<CarRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
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
                return false;
            }
        }
    }
}
