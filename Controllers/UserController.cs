using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;

namespace Zwierzyniec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            return Ok();
        }
        [HttpPost("create")]
        public IActionResult CreateUser(UserAdd userAddModel)
        {
            return Ok();
        }
        [HttpPut("update/{userId}")]
        public IActionResult UpdateUser(UserEdit userEditModel)
        {
            return Ok();
        }
        [HttpDelete("delete/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            return Ok();
        }
    }
}
