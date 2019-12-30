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

    [Route("api/cars")]
    [ApiController, Produces("application/json")]
    public class CarsController : ControllerBase
    {
        private readonly ICarsService carsService;

        private readonly ILogger<CarsController> logger;

        public CarsController(
            ICarsService carsService, 
            ILogger<CarsController> logger)
        {
            this.carsService = carsService;
            this.logger = logger;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
        {
            try
            {
                List<Brand> brands = await this.carsService.GetAllBrands();
                return Ok(brands);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("brands/models")]
        public async Task<ActionResult<IEnumerable<BrandModel>>> GetModelsForBrand([FromQuery] string brandHttpPath)
        {
            try
            {
                List<BrandModel> brandModels = await this.carsService.GetAllBrandModels(brandHttpPath);

                return Ok(brandModels);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("brands/models/cars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsForModel([FromQuery] string modelHttpPath)
        {
            try
            {
                List<Car> modelCars = await this.carsService.GetAllCarsForModel(modelHttpPath);
                return Ok(modelCars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}