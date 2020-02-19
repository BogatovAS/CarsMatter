using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IBrandsRepository<T>
    {
        Task<List<T>> GetAllBrands();

        Task AddBrand(T brand);

        Task UpdateBrand(T brand);

        Task DeleteBrand(string brandId);
    }
}
