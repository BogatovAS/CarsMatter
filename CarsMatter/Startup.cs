using CarsMatter.Infrastructure.Authentication;
using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Jobs;
using CarsMatter.Infrastructure.Repository;
using CarsMatter.Infrastructure.Services;
using Hangfire.MemoryStorage;
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

            services.AddMemoryCache();
            services.AddCors();

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
    }
}
