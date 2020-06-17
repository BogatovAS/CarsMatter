using CarsMatter.Infrastructure.Models.MsSQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);

        Task<string> GetUserIdByUsername(string username);

        Task<List<MyCar>> GetMyCars(string userId);

        Task<MyCar> GetSelectedCar(string userId);

        Task<MyCar> SetSelectedCar(string userId, string myCarId);

        Task<MyCar> AddCar(MyCar car);

        Task<MyCar> UpdateCar(MyCar car);

        Task<bool> DeleteCar(string userId, string userCarId);

        Task<User> Create(User user, string password);

        Task Delete(string id);
    }
}
