using CarsMatter.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IBrandModelsRepository
    {
        Task<List<BrandModel>> GetAllBrandModels(int brandId);

        Task<BrandModel> AddBrandModel(BrandModel brandModel);

        Task<BrandModel> UpdateBrandModel(BrandModel brandModel);

        Task<bool> DeleteBrandModel(int carId);
    }
}
