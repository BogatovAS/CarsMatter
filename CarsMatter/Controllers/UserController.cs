namespace CarsMatter.Controllers
{
    using CarsMatter.Infrastructure.Interfaces;
    using CarsMatter.Infrastructure.Models;
    using CarsMatter.Infrastructure.Models.MsSQL;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;


    [Route("api/user")]
    [ApiController, Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost("logIn")]
        public async Task<ActionResult<bool>> LogIn([FromBody] UserRequestModel user)
        {
            try
            {
                var result = await this.userService.Authenticate(user.Username, user.Password);

                if (result)
                {
                    return this.Ok(result);
                }

                return BadRequest("Неверные имя пользователя или пароль");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<bool>> SignUp([FromBody] UserRequestModel user)
        {
            User newUser = new User();
            newUser.Username = user.Username;

            try
            {
                var createdUser = await this.userService.Create(newUser, user.Password);
                return this.Ok(createdUser != null);
            }
            catch(Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await this.userService.Delete(id);
                return this.Ok();
            }
            catch(Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("cars")]
        public async Task<ActionResult<List<MyCar>>> GetUserCars()
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                var userCars = await userService.GetMyCars(userId);

                return this.Ok(userCars);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("selectedCar")]
        public async Task<ActionResult<List<MyCar>>> GetSelectedCar()
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await userService.GetSelectedCar(userId);

                return this.Ok(selectedCar);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("selectedCar")]
        public async Task<ActionResult<List<MyCar>>> SelectCar([FromQuery] string userCarId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await userService.SetSelectedCar(userId, userCarId);

                return this.Ok(selectedCar);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("car")]
        public async Task<ActionResult<MyCar>> AddUserCar([FromBody] MyCar userCar)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                userCar.UserId = userId;

                MyCar selectedCar = await userService.AddCar(userCar);

                return this.Ok(selectedCar);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("car")]
        public async Task<ActionResult<MyCar>> UpdateUserCar([FromBody] MyCar userCar)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                userCar.UserId = userId;

                MyCar selectedCar = await userService.UpdateCar(userCar);

                return this.Ok(selectedCar);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
