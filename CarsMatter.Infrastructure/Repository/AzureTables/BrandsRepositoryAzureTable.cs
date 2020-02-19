using System.Collections.Generic;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;

namespace CarsMatter.Infrastructure.Repository.AzureTables
{
    public class BrandsRepositoryAzureTable : IBrandsRepository<Brand>
    {
        private readonly IAzureTable<Brand> brandTable;

        public BrandsRepositoryAzureTable(IAzureTable<Brand> brandTable)
        {
            this.brandTable = brandTable;
        }

        public Task AddBrand(Brand brand)
        {
            return this.brandTable.Insert(brand);
        }

        public Task DeleteBrand(string brandId)
        {
            return this.brandTable.Delete(brandId, brandId);
        }

        public Task<List<Brand>> GetAllBrands()
        {
            return this.brandTable.GetList();
        }

        public Task UpdateBrand(Brand brand)
        {
            return this.brandTable.Update(brand);
        }
    }
}
