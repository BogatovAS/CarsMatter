using CarsMatter.Infrastructure.Authentication;
using CarsMatter.Infrastructure.Db;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Jobs;
using CarsMatter.Infrastructure.Repository;
using CarsMatter.Infrastructure.Services;
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
    using Hangfire.MemoryStorage;

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

            // Use a PostgreSQL database
            var sqlConnectionString = this.configuration.GetSection("CarsMatterConnectionString").Value;

            services.AddEntityFrameworkNpgsql().AddDbContext<CarsMatterDbContext>(options =>
                options.UseNpgsql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("AspNet5MultipleProject")
                ), ServiceLifetime.Transient, ServiceLifetime.Transient);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Cars Matter API", Version = "v1" });
            });

            services.AddTransient<IRefillNotesRepository, RefillNotesRepository>();
            services.AddTransient<IConsumablesNotesRepository, ConsumablesNotesRepository>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBrandsRepository, BrandsRepository>();
            services.AddTransient<IBrandModelsRepository, BrandModelsRepository>();
            services.AddTransient<ICarsRepository, CarsRepository>();
            services.AddTransient<IFavoriteCarsRepository, FavoriteCarsRepository>();

            services.AddHttpClient<ICarsService, CarsService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://avtomarket.ru");
            });

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
            RecurringJob.AddOrUpdate<UpdateAllCarsJob>("updateAllCarsJob", job => job.Run(), Cron.Daily);

            app.UseMvc();
        }
    }
}
