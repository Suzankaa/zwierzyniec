using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;
using Zwierzyniec.Models.Output;
using Zwierzyniec.Services;

namespace Zwierzyniec.Controllers
{
    /// <summary>
    /// Endpointy uzytkownikow z danymi w pamieci.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMockDataService _data;

        public UserController(IMockDataService data)
        {
            _data = data;
        }

        /// <summary>
        /// Pobiera pojedynczego uzytkownika po Id.
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserResponse> GetUserById(int userId)
        {
            var user = _data.GetUserById(userId);
            return user is null ? NotFound("Uzytkownik nie znaleziony") : Ok(user);
        }

        /// <summary>
        /// Tworzy nowego uzytkownika.
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        public ActionResult<UserResponse> CreateUser([FromBody] UserAdd userAddModel)
        {
            var created = _data.CreateUser(userAddModel);
            Response.Headers["X-Message"] = "Uzytkownik dodany";
            return CreatedAtAction(nameof(GetUserById), new { userId = created.Id }, created);
        }

        /// <summary>
        /// Aktualizuje dane uzytkownika po Id.
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserResponse> UpdateUser([FromBody] UserEdit userEditModel)
        {
            var updated = _data.UpdateUser(userEditModel);
            return updated is null ? NotFound("Uzytkownik nie znaleziony") : Ok(updated);
        }

        /// <summary>
        /// Usuwa uzytkownika po Id.
        /// </summary>
        [HttpDelete("delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUser(int userId)
        {
            return _data.DeleteUser(userId) ? NoContent() : NotFound("Uzytkownik nie znaleziony");
        }
    }
}
