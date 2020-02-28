namespace CarsMatter.Controllers
{
    using CarsMatter.Infrastructure.Interfaces;
    using CarsMatter.Infrastructure.Models.MsSQL;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/favorite_cars")]
    [ApiController, Produces("application/json")]
    public class FavoriteCarsController : ControllerBase
    {
        private readonly IFavoriteCarsRepository<Car> favoriteCarsRepository;
        private readonly ICarsRepository<Car> carsRepository;
        private readonly ILogger<FavoriteCarsController> logger;

        public FavoriteCarsController(
            IFavoriteCarsRepository<Car> favoriteCarsRepository,
            ICarsRepository<Car> carsRepository,
            ILogger<FavoriteCarsController> logger)
        {
            this.favoriteCarsRepository = favoriteCarsRepository;
            this.carsRepository = carsRepository;
            this.logger = logger;
        }

        [HttpPost("{carId}")]
        public async Task<ActionResult<bool>> AddCarToFavorites([FromRoute] string carId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                await this.favoriteCarsRepository.Add(userId, carId);
                return Ok(true);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{carId}")]
        public async Task<ActionResult<bool>> RemoveCarFromFavorites([FromRoute] string carId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                await this.favoriteCarsRepository.Delete(userId, carId);
                return Ok(true);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{carId}")]
        public async Task<ActionResult<bool>> IsFavoriteCar([FromRoute] string carId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                bool response = await this.favoriteCarsRepository.IsFavoriteCar(userId, carId);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetFavoriteCars()
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                List<Car> favoriteCars = await this.favoriteCarsRepository.GetFavoriteCars(userId);

                foreach (var car in favoriteCars)
                {
                    car.Base64CarImage = await this.carsRepository.GetImageForModel(car.CarImagePath);
                }

                return Ok(favoriteCars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
