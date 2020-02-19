using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IFavoriteCarsRepository<T>
    {
        Task<List<T>> GetFavoriteCars(string userId);

        Task<bool> IsFavoriteCar(string userId, string carId);

        Task Add(string userId, string carId);

        Task Delete(string userId, string carId);
    }
}
