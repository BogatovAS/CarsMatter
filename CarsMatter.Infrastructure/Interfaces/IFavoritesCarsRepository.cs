using CarsMatter.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IFavoriteCarsRepository
    {
        Task<List<Car>> GetFavoriteCars(int userId);

        Task<bool> Add(int userId, int carId);

        Task<bool> Delete(int favoriteRecordId);
    }
}
