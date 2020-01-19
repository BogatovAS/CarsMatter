using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);

        Task<int> GetUserIdByUsername(string username);
    }
}
