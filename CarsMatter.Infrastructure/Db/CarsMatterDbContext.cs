using CarsMatter.Infrastructure.Models.Journal;
using Microsoft.EntityFrameworkCore;

namespace CarsMatter.Infrastructure.Db
{
    public class CarsMatterDbContext : DbContext
    {
        public DbSet<RefillNote> RefillNotes { get; set; }

        public DbSet<ConsumablesNote> ConsumablesNotes { get; set; }

        public CarsMatterDbContext(DbContextOptions<CarsMatterDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
