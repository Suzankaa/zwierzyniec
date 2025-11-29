using Microsoft.AspNetCore.Mvc;

namespace Zwierzyniec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        [HttpPost("create")]
        public IActionResult CreateOrder()
        {
            return Ok();
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(int orderId)
        {
            return Ok();
        }

        [HttpDelete("cancel/{orderId}")]
        public IActionResult CancelOrder(int orderId)
        {
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            return Ok();
        }

        [HttpGet("status/{status}")]
        public IActionResult GetOrdersByStatus(string status)
        {
            return Ok();
        }
    }
}
