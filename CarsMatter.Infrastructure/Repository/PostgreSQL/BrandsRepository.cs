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
    public class BrandsRepository : IBrandsRepository<Brand>
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<BrandsRepository> logger;

        public BrandsRepository(CarsMatterDbContext dbContext, ILogger<BrandsRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await Task.Run(() => this.dbContext.Brands.OrderBy(brand => brand.BrandName).ToList());
        }

        public async Task AddBrand(Brand brand)
        {
            this.dbContext.Brands.Add(brand);
            await this.SaveChanges();
        }

        public async Task DeleteBrand(string brandId)
        {
            Brand brand = this.dbContext.Brands.FirstOrDefault(br => br.Id == brandId);
            this.dbContext.Brands.Remove(brand);
            await this.SaveChanges();
        }

        public async Task UpdateBrand(Brand brand)
        {
            EntityEntry<Brand> updatedBrand;
            Brand existingBrand = this.dbContext.Brands.FirstOrDefault(br => br.BrandName == brand.BrandName);

            if (existingBrand != null)
            {
                updatedBrand = this.dbContext.Brands.Update(existingBrand);
            }
            else
            {
                updatedBrand = this.dbContext.Brands.Add(brand);
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
