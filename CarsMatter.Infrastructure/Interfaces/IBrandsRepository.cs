using CarsMatter.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IBrandsRepository
    {
        Task<List<Brand>> GetAllBrands();

        Task<Brand> AddBrand(Brand brand);

        Task<Brand> UpdateBrand(Brand brand);

        Task<bool> DeleteBrand(int brandId);
    }
}
