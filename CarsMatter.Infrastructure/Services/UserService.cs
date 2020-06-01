using CarsMatter.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using CarsMatter.Infrastructure.Models.MsSQL;
using System.Collections.Generic;
using System;
using CarsMatter.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<MyCar>> GetMyCars(string userId)
        {
            return await Task.Run(() =>
            {
                var user = this.dbContext.Users.Include(u => u.MyCars).FirstOrDefault(u => u.Id == userId);

                return user.MyCars.ToList();
            });
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Поле 'Пароль' и 'Имя пользоватея' должно быть заполнено");
            }

            var user = this.dbContext.Users.Where(user => user.Username == username).FirstOrDefault();

            if (user == null)
            {
                throw new Exception($"Пользоатель с таким именем не найден: '{user.Username}'");
            }

            if (!VerifyPasswordHash(password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetUserIdByUsername(string username)
        {
            User user = await Task.Run(() => this.dbContext.Users.Where(user => user.Username == username).FirstOrDefault());

            return user.Id;
        }

        public async Task<User> Create(User user, string password)
        {

            if (this.dbContext.Users.Where(u => u.Username == user.Username).Any())
            {
                throw new Exception($"Имя пользователя '{user.Username}' уже занято");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);

            this.dbContext.Users.Add(user);
            await this.dbContext.SaveChangesAsync();

            return user;
        }

        public async Task Delete(string id)
        {
            User user = this.dbContext.Users.Where(user => user.Id == id).FirstOrDefault();
            if (user != null)
            {
                this.dbContext.Users.Remove(user);
                await this.dbContext.SaveChangesAsync();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Поле 'Пароль' должно быть заполнено");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Поле 'Пароль' должно быть заполнено");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Неправильная длина хэша пароля (ожидается 64 байта)");
            }
            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Неправильная длина соли пароля (ожидается 128 байт)");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
