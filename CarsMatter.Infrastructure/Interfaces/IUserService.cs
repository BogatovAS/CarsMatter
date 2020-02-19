using CarsMatter.Infrastructure.Models.MsSQL;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);

        Task<string> GetUserIdByUsername(string username);

        Task<User> Create(User user, string password);

        Task Delete(string id);
    }
}
