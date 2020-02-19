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

namespace CarsMatter.Controllers
{
    [Authorize]
    [Route("api/favorite_cars")]
    [ApiController, Produces("application/json")]
    public class FavoriteCarsController : ControllerBase
    {
        private readonly IFavoriteCarsRepository<FavoriteCar> favoriteCarsRepository;
        private readonly ICarsRepository<Car> carsRepository;
        private readonly ILogger<FavoriteCarsController> logger;

        public FavoriteCarsController(
            IFavoriteCarsRepository<FavoriteCar> favoriteCarsRepository,
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
                await this.favoriteCarsRepository.Add(carId, userId);
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
                await this.favoriteCarsRepository.Delete(carId, userId);
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
                bool response = await this.favoriteCarsRepository.IsFavoriteCar(carId, userId);
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
                List<FavoriteCar> favoriteCars = await this.favoriteCarsRepository.GetFavoriteCars(userId);

                List<Car> cars = favoriteCars.Select(favoriteCar => favoriteCar.Car).ToList();

                foreach (var car in cars)
                {
                    car.Base64CarImage = await this.carsRepository.GetImageForModel(car.CarImagePath);
                }

                return Ok(cars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
