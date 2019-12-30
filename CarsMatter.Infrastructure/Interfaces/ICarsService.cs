using CarsMatter.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface ICarsService
    {
        Task<List<Brand>> GetAllBrands();

        Task<List<BrandModel>> GetAllBrandModels(string brandHttpPath);

        Task<List<Model>> GetAllCarsForModel(string carModelHttpPath);

        Task<List<Car>> GetAllCarsModificationsForModel(string carModificationHttpPath);

        Task<string> GetImageForModel(string modelImageHttpPath);
    }
}
