namespace CarsMatter.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CarsMatter.Infrastructure.Models.MsSQL;
    using CarsMatter.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using System.Linq;

    [Route("api/cars")]
    [ApiController, Produces("application/json")]
    public class CarsController : ControllerBase
    {
        private readonly IBrandsRepository<Brand> brandsRepository;
        private readonly IBrandModelsRepository<BrandModel> brandModelsRepository;
        private readonly ICarsRepository<Car> carsRepository;

        private readonly ILogger<CarsController> logger;

        public CarsController(
            IBrandsRepository<Brand> brandsRepository,
            IBrandModelsRepository<BrandModel> brandModelsRepository,
            ICarsRepository<Car> carsRepository,
            ILogger<CarsController> logger)
        {
            this.brandsRepository = brandsRepository;
            this.brandModelsRepository = brandModelsRepository;
            this.carsRepository = carsRepository;
            this.logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchCars(
            [FromQuery] string carName = null,
            [FromQuery] int? lowPrice = null,
            [FromQuery] int? highPrice = null,
            [FromQuery] string manufactureStartDate = null,
            [FromQuery] string manufactureEndDate = null)
        {

            bool inPrice(Car car)
            {
                bool lowPriceCorrect = !lowPrice.HasValue
                   ? true
                   : car.LowPrice > lowPrice;

                bool highPriceCorrect = !highPrice.HasValue
                    ? true
                    : car.HighPrice < highPrice;

                return lowPriceCorrect && highPriceCorrect;
            }

            bool inDates(Car car)
            {
                bool leftDateCorrect = string.IsNullOrEmpty(manufactureStartDate)
                    ? true
                    : int.Parse(car.ManufactureStartDate) > int.Parse(manufactureStartDate);

                string carManufactureEndDate = car.ManufactureEndDate;
                if (car.ManufactureEndDate == "в производстве")
                {
                    carManufactureEndDate = DateTime.MaxValue.Year.ToString();
                }

                bool rightDateCorrect = string.IsNullOrEmpty(manufactureEndDate)
                    ? true
                    : int.Parse(carManufactureEndDate) < int.Parse(manufactureEndDate);

                return leftDateCorrect && rightDateCorrect;
            }

            bool inName(Car car)
            {
                return string.IsNullOrEmpty(carName) ? true : car.CarName.Contains(carName, StringComparison.InvariantCultureIgnoreCase) || carName.Contains(car.CarName, StringComparison.InvariantCultureIgnoreCase);
            }

            List<Car> allCars = await this.carsRepository.GetAllCars();

            var filteredCars = allCars
                .Where(inPrice)
                .Where(inDates)
                .Where(inName)
                .ToList();

            foreach (var car in filteredCars)
            {
                car.Base64CarImage = await this.carsRepository.GetImageForModel(car.CarImagePath);
            }

            return Ok(filteredCars.OrderBy(car => car.CarName));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
        {
            try
            {
                List<Brand> brands = await this.brandsRepository.GetAllBrands();
                return Ok(brands);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("brands/{brandId}/models")]
        public async Task<ActionResult<IEnumerable<BrandModel>>> GetModelsForBrand([FromRoute] string brandId)
        {
            try
            {
                List<BrandModel> brandModels = await this.brandModelsRepository.GetAllBrandModels(brandId);

                return Ok(brandModels);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("brands/models/{brandModelId}/cars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsForModel([FromRoute] string brandModelId)
        {
            try
            {
                List<Car> modelCars = await this.carsRepository.GetAllCars(brandModelId);

                foreach (var car in modelCars) 
                {
                    car.Base64CarImage = await this.carsRepository.GetImageForModel(car.CarImagePath);
                }

                return Ok(modelCars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("brand/models/cars/recognize")]
        public async Task<ActionResult<Car>> RecognizeCar([FromBody] string base64CarImage)
        {
            string recognitionUrl = "https://automl.googleapis.com/v1beta1/projects/651771669084/locations/us-central1/models/ICN2461371127985864704:predict";
            string header = "Bearer ";

            JObject body = new JObject
            {
                {
                    "payload", new JObject {
                        {
                            "image", new JObject {
                                {
                                    "imageBytes", base64CarImage
                                }
                            }
                        }
                    }
                }
            };
            return new Car();
        }
    }
}