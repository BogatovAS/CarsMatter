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
    public class BrandModelsRepository : IBrandModelsRepository
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<BrandModelsRepository> logger;

        public BrandModelsRepository(CarsMatterDbContext dbContext, ILogger<BrandModelsRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<BrandModel>> GetAllBrandModels(int brandId)
        {
            return await Task.Run(() => this.dbContext.BrandModels.Where(brandModel => brandModel.BrandId == brandId).ToList());
        }

        public async Task<BrandModel> AddBrandModel(BrandModel brandModel)
        {
            var createdBrandModel = this.dbContext.BrandModels.Add(brandModel);
            await this.SaveChanges();
            return createdBrandModel.Entity;
        }

        public async Task<bool> DeleteBrandModel(int brandModelId)
        {
            BrandModel brandModel = this.dbContext.BrandModels.FirstOrDefault(bm => bm.Id == brandModelId);
            this.dbContext.BrandModels.Remove(brandModel);
            return await this.SaveChanges();
        }

        public async  Task<BrandModel> UpdateBrandModel(BrandModel brandModel)
        {
            EntityEntry<BrandModel> updatedBrandModel;
            BrandModel existingBrandModel = this.dbContext.BrandModels.FirstOrDefault(bm => bm.Id == brandModel.Id);

            if(existingBrandModel != null)
            {
                updatedBrandModel = this.dbContext.BrandModels.Update(existingBrandModel);
            }
            else
            {
                updatedBrandModel = this.dbContext.BrandModels.Add(brandModel);
            }
            await this.SaveChanges();
            return updatedBrandModel.Entity;
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
