using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface ICarsRepository<T>
    {
        Task<List<T>> GetAllCars(string brandModelId);

        Task<List<T>> GetAllCars();

        Task AddCar(T car);

        Task UpdateCar(T car);

        Task DeleteCar(string carId);

        Task<string> GetImageForModel(string modelImageHttpPath);
    }
}
