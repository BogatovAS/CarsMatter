using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using CarsMatter.Infrastructure.Models;

namespace CarsMatter.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<UserService> logger;

        public UserService(CarsMatterDbContext dbContext, ILogger<UserService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            return await Task.Run(() => this.dbContext.Users.Any(user => user.Username == username && user.Password == password));
        }

        public async Task<int> GetUserIdByUsername(string username)
        {
            User user = this.dbContext.Users.FirstOrDefault(user => user.Username == username);

            return await Task.FromResult(user.Id);
        }
    }
}
