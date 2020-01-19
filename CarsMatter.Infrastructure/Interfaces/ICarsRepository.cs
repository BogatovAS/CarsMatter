using CarsMatter.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface ICarsRepository
    {
        Task<List<Car>> GetAllCars(int brandModelId);

        Task<Car> AddCar(Car car);

        Task<Car> UpdateCar(Car car);

        Task<bool> DeleteCar(int carId);
    }
}
