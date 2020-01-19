namespace CarsMatter.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CarsMatter.Infrastructure.Models;
    using CarsMatter.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/cars")]
    [ApiController, Produces("application/json")]
    public class CarsController : ControllerBase
    {
        private readonly IBrandsRepository brandsRepository;
        private readonly IBrandModelsRepository brandModelsRepository;
        private readonly ICarsRepository carsRepository;
        private readonly IFavoriteCarsRepository FavoriteCarsRepository;
        private readonly IUserService userService;

        private readonly ILogger<CarsController> logger;

        public CarsController(
            IBrandsRepository brandsRepository,
            IBrandModelsRepository brandModelsRepository,
            ICarsRepository carsRepository,
            IFavoriteCarsRepository FavoriteCarsRepository,
            IUserService userService,
            ILogger<CarsController> logger)
        {
            this.brandsRepository = brandsRepository;
            this.brandModelsRepository = brandModelsRepository;
            this.carsRepository = carsRepository;
            this.FavoriteCarsRepository = FavoriteCarsRepository;
            this.userService = userService;
            this.logger = logger;
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

        [HttpGet("brands/models")]
        public async Task<ActionResult<IEnumerable<BrandModel>>> GetModelsForBrand([FromQuery] int brandId)
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

        [HttpGet("brands/models/cars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsForModel([FromQuery] int brandModelId)
        {
            try
            {
                List<Car> modelCars = await this.carsRepository.GetAllCars(brandModelId);
                return Ok(modelCars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost("brand/models/cars")]
        public async Task<ActionResult<bool>> AddCarToFavorite([FromQuery] int carId)
        {
            try
            {
                string username = this.Request.Headers["Username"].ToString();

                int userId = await this.userService.GetUserIdByUsername(username);
                bool response = await this.FavoriteCarsRepository.Add(carId, userId);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}