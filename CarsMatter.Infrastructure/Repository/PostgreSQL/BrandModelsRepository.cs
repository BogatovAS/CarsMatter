using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.MsSQL;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace CarsMatter.Infrastructure.Repository
{
    public class BrandModelsRepository : IBrandModelsRepository<BrandModel>
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<BrandModelsRepository> logger;

        public BrandModelsRepository(CarsMatterDbContext dbContext, ILogger<BrandModelsRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<BrandModel>> GetAllBrandModels(string brandId)
        {
            return await Task.Run(() => this.dbContext.BrandModels
            .Where(brandModel => brandModel.BrandId == brandId)
            .OrderBy(brandModel => brandModel.ModelName)
            .ToList());
        }

        public async Task AddBrandModel(BrandModel brandModel)
        {
            this.dbContext.BrandModels.Add(brandModel);
            await this.SaveChanges();
        }

        public async Task DeleteBrandModel(string brandModelId)
        {
            BrandModel brandModel = this.dbContext.BrandModels.FirstOrDefault(bm => bm.Id == brandModelId);
            this.dbContext.BrandModels.Remove(brandModel);
            await this.SaveChanges();
        }

        public async  Task UpdateBrandModel(BrandModel brandModel)
        {
            EntityEntry<BrandModel> updatedBrandModel;
            BrandModel existingBrandModel = this.dbContext.BrandModels.FirstOrDefault(bm => bm.ModelName == brandModel.ModelName);

            if(existingBrandModel != null)
            {
                updatedBrandModel = this.dbContext.BrandModels.Update(existingBrandModel);
            }
            else
            {
                updatedBrandModel = this.dbContext.BrandModels.Add(brandModel);
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
    }
}
