﻿using CarsMatter.Infrastructure.Models;
using CarsMatter.Infrastructure.Models.Journal;
using Microsoft.EntityFrameworkCore;

namespace CarsMatter.Infrastructure.Db
{
    public sealed class CarsMatterDbContext : DbContext
    {
        public DbSet<RefillNote> RefillNotes { get; set; }

        public DbSet<ConsumablesNote> ConsumablesNotes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<BrandModel> BrandModels { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<FavoriteCar> FavoriteCars { get; set; }

        public CarsMatterDbContext(DbContextOptions<CarsMatterDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
