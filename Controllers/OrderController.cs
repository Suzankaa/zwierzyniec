using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;
using Zwierzyniec.Models.Output;
using Zwierzyniec.Services;

namespace Zwierzyniec.Controllers
{
    /// <summary>
    /// Endpointy zamowien z danymi w pamieci.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMockDataService _data;

        public OrderController(IMockDataService data)
        {
            _data = data;
        }

        /// <summary>
        /// Tworzy nowe zamowienie.
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        public ActionResult<OrderResponse> CreateOrder([FromBody] OrderAdd order)
        {
            var created = _data.CreateOrder(order);
            Response.Headers["X-Message"] = "Zamowienie utworzone";
            return CreatedAtAction(nameof(GetOrderById), new { orderId = created.Id }, created);
        }

        /// <summary>
        /// Pobiera pojedyncze zamowienie po Id.
        /// </summary>
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderResponse> GetOrderById(int orderId)
        {
            var order = _data.GetOrderById(orderId);
            return order is null ? NotFound("Zamowienie nie znalezione") : Ok(order);
        }

        /// <summary>
        /// Anuluje zamowienie po Id (ustawia status cancelled).
        /// </summary>
        [HttpDelete("cancel/{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CancelOrder(int orderId)
        {
            return _data.CancelOrder(orderId) ? NoContent() : NotFound("Zamowienie nie znalezione");
        }

        /// <summary>
        /// Zwraca zamowienia wybranego uzytkownika.
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderResponse>> GetOrdersByUserId(int userId)
        {
            return Ok(_data.GetOrdersByUserId(userId));
        }

        /// <summary>
        /// Zwraca zamowienia o wskazanym statusie.
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderResponse>> GetOrdersByStatus(string status)
        {
            return Ok(_data.GetOrdersByStatus(status));
        }
    }
}
