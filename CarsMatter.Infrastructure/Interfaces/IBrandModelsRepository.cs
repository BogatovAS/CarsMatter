using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IBrandModelsRepository<T>
    {
        Task<List<T>> GetAllBrandModels(string brandId);

        Task AddBrandModel(T brandModel);

        Task UpdateBrandModel(T brandModel);

        Task DeleteBrandModel(string brandModelId);
    }
}
