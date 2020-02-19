using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models;
using CarsMatter.Infrastructure.Models.MsSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CarsMatter.Controllers
{
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
        public async Task<ActionResult<User>> SignUp([FromBody] UserRequestModel user)
        {
            User newUser = new User();
            newUser.Username = user.Username;

            try
            {
                var createdUser = await this.userService.Create(newUser, user.Password);
                return this.Ok(createdUser);
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
    }
}
