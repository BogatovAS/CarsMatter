using CarsMatter.Infrastructure.Authentication;
using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Jobs;
using CarsMatter.Infrastructure.Repository;
using CarsMatter.Infrastructure.Services;
using CarsMatter.Configs;
using CarsMatter.Infrastructure.Models.AzureTables;
using CarsMatter.Infrastructure.Repository.AzureTables;
using CarsMatter.Infrastructure.Storages;
using CarsMatter.Initializers;
using Hangfire.MemoryStorage;
using Microsoft.WindowsAzure.Storage;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace CarsMatter
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(hangfireConfiguration =>
            {
                hangfireConfiguration
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMemoryStorage();
            });
            services.AddHangfireServer();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Use a MsSQL database
            var sqlConnectionString = this.configuration.GetSection("CarsMatterConnectionString").Value;

            services.AddEntityFrameworkSqlServer().AddDbContext<CarsMatterDbContext>(options =>
                options.UseSqlServer(
                    sqlConnectionString
                ), ServiceLifetime.Transient, ServiceLifetime.Transient);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Cars Matter API", Version = "v1" });
            });

            services.AddHttpClient("avtomarket", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://avtomarket.ru");
            });


            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IBrandsRepository<Infrastructure.Models.MsSQL.Brand>, BrandsRepository>();
            services.AddTransient<IBrandModelsRepository<Infrastructure.Models.MsSQL.BrandModel>, BrandModelsRepository>();
            services.AddTransient<ICarsRepository<Infrastructure.Models.MsSQL.Car>, CarsRepository>();
            services.AddTransient<IFavoriteCarsRepository<Infrastructure.Models.MsSQL.Car>, FavoriteCarsRepository>();
            services.AddTransient<IRefillNotesRepository<Infrastructure.Models.MsSQL.RefillNote>, RefillNotesRepository>();
            services.AddTransient<IConsumablesNotesRepository<Infrastructure.Models.MsSQL.ConsumablesNote>, ConsumablesNotesRepository>();

            services.AddTransient<IBrandsRepository<Infrastructure.Models.AzureTables.Brand>, BrandsRepositoryAzureTable>();
            services.AddTransient<IBrandModelsRepository<Infrastructure.Models.AzureTables.BrandModel>, BrandModelsRepositoryAzureTable>();
            services.AddTransient<ICarsRepository<Infrastructure.Models.AzureTables.Car>, CarsRepositoryAzureTable>();
            services.AddTransient<IFavoriteCarsRepository<Infrastructure.Models.AzureTables.FavoriteCar>, FavoriteCarsRepositoryAzureTable>();
            services.AddTransient<IRefillNotesRepository<Infrastructure.Models.AzureTables.RefillNote>, RefillNotesRepositoryAzureTable>();
            services.AddTransient<IConsumablesNotesRepository<Infrastructure.Models.AzureTables.ConsumablesNote>, ConsumablesNotesRepositoryAzureTable>();

            services.AddMemoryCache();
            services.AddCors();

            // Use it if should move to AzureStorage database
            //AddCloudStorageAccount(services);
            //services.AddAsyncInitializer<AzureTablesInitializer>();

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car finder API V1");
            });

            app.UseCors(builder => builder.AllowAnyOrigin());

            RewriteOptions rewriteOptions = new RewriteOptions();
            rewriteOptions.AddRedirect("^$", "swagger");

            app.UseRewriter(rewriteOptions);
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<UpdateAllCarsJob>("updateAllCarsJob", job => job.Run(), Cron.Daily(21));

            app.UseMvc();
        }

        public void AddCloudStorageAccount(IServiceCollection services)
        {
            services.AddSingleton<CloudStorageAccount>(serviceProvider =>
            {
                StorageAccountConfig config = configuration.GetSection("StorageAccountConfig").Get<StorageAccountConfig>();

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
                {
                    return CloudStorageAccount.Parse(config.DevConnectionString);
                }

                return CloudStorageAccount.Parse(config.ConnectionString);
            });
            services.AddSingleton<IAzureTable<Brand>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<Brand>(storageAccount, "brands");
            });
            services.AddScoped<IAzureTable<BrandModel>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<BrandModel>(storageAccount, "brandModels");
            });
            services.AddScoped<IAzureTable<Car>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<Car>(storageAccount, "cars");
            });
            services.AddScoped<IAzureTable<FavoriteCar>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<FavoriteCar>(storageAccount, "favoriteCars");
            });
            services.AddScoped<IAzureTable<User>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<User>(storageAccount, "users");
            });
            services.AddScoped<IAzureTable<RefillNote>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<RefillNote>(storageAccount, "refillNotes");
            });
            services.AddScoped<IAzureTable<ConsumablesNote>>(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return new AzureTable<ConsumablesNote>(storageAccount, "consumablesNotes");
            });
        }
    }
}
