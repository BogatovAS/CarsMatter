namespace CarsMatter.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CarsMatter.Infrastructure.Models;
    using CarsMatter.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using CarsMatter.Infrastructure.Helpers;
    using System;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController, Produces("application/json")]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository carRepository;

        private readonly ILogger<CarsController> logger;

        public CarsController(ICarRepository carRepository, ILogger<CarsController> logger)
        {
            this.carRepository = carRepository;
            this.logger = logger;
        }

        [HttpGet("/brands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
        {
            try
            {
                List<Brand> brands = await CarsHtmlParser.ParseBrands();
                return Ok(brands);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("/brands/models")]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetModelsForBrand([FromQuery] string brandHttpPath)
        {
            try
            {
                List<CarModel> brandModels = await CarsHtmlParser.ParseModel(brandHttpPath);

                return Ok(brandModels);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("/brands/models/cars")]
        public async Task<ActionResult<IEnumerable<Model>>> GetCarsForModel([FromQuery] string modelHttpPath)
        {
            try
            {
                List<Model> brandCars = await CarsHtmlParser.ParseCarsForModel(modelHttpPath);
                return Ok(brandCars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetCars()
        {
            var cars = await CarsHtmlParser.GetAllCars();

            return Ok(cars);
        }
    }
}