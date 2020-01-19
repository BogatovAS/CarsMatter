using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Jobs
{
    public class UpdateAllCarsJob
    {
        private readonly ICarsService carsService;

        private readonly IBrandsRepository brandsRepository;
        private readonly IBrandModelsRepository brandModelsRepository;
        private readonly ICarsRepository carsRepository;

        public UpdateAllCarsJob(
            ICarsService carsService,
            IBrandsRepository brandsRepository,
            IBrandModelsRepository brandModelsRepository,
            ICarsRepository carsRepository)
        {
            this.carsService = carsService;
            this.brandModelsRepository = brandModelsRepository;
            this.carsRepository = carsRepository;
            this.brandsRepository = brandsRepository;
        }

        public async Task Run()
        {
            List<Brand> brands = await this.carsService.GetAllBrands();

            foreach (Brand brand in brands)
            {
                await this.brandsRepository.UpdateBrand(brand);

                List<BrandModel> brandModels = await this.carsService.GetAllBrandModels(brand.HttpPath);

                foreach (BrandModel brandModel in brandModels)
                {
                    brandModel.BrandId = brand.Id;
                    await this.brandModelsRepository.UpdateBrandModel(brandModel);

                    List<Car> cars = await this.carsService.GetAllCarsForModel(brandModel.HttpPath);

                    foreach(Car car in cars)
                    {
                        car.BrandModelId = brandModel.Id;
                        await this.carsRepository.UpdateCar(car);
                    }
                }
            }
        }
    }
}
