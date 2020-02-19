using System.Collections.Generic;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;

namespace CarsMatter.Infrastructure.Repository.AzureTables
{
    public class BrandModelsRepositoryAzureTable : IBrandModelsRepository<BrandModel>
    {
        private readonly IAzureTable<BrandModel> brandModelTable;

        public BrandModelsRepositoryAzureTable(IAzureTable<BrandModel> brandModelTable)
        {
            this.brandModelTable = brandModelTable;
        }

        public Task AddBrandModel(BrandModel brandModel)
        {
            return this.brandModelTable.Insert(brandModel);
        }

        public Task DeleteBrandModel(string brandModelId)
        {
            return this.brandModelTable.Delete(brandModelId, brandModelId);
        }

        public Task<List<BrandModel>> GetAllBrandModels(string brandId)
        {
            return this.brandModelTable.GetList(nameof(BrandModel.BrandId), brandId);
        }

        public Task UpdateBrandModel(BrandModel brandModel)
        {
            return this.brandModelTable.Update(brandModel);
        }
    }
}
