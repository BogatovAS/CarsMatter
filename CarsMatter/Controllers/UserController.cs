using CarsMatter.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarsMatter.Controllers
{
    [Route("api/user")]
    [ApiController, Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> LogIn(string username, string password)
        {
            return await this.userService.Authenticate(username, password);
        }
    }
}
