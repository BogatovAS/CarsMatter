using CarsMatter.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using static CarsMatter.Infrastructure.Models.Car;

namespace CarsMatter.Infrastructure.Db
{
    public class CarsMatterDbContext : DbContext
    {
        public CarsMatterDbContext(DbContextOptions<CarsMatterDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
