using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;
using Zwierzyniec.Models.Output;
using Zwierzyniec.Services;

namespace Zwierzyniec.Controllers
{
    /// <summary>
    /// Endpointy produktowe z danymi w pamieci.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class ProductsController : ControllerBase
    {
        private readonly IMockDataService _data;

        public ProductsController(IMockDataService data)
        {
            _data = data;
        }

        /// <summary>
        /// Dodaje nowy produkt.
        /// </summary>
        [HttpPut("add")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        public ActionResult<ProductResponse> AddProduct([FromBody] ProductAdd product)
        {
            var created = _data.AddProduct(product);
            Response.Headers["X-Message"] = "Produkt dodany";
            return CreatedAtAction(nameof(GetProductById), new { Id = created.Id }, created);
        }

        /// <summary>
        /// Aktualizuje produkt po Id.
        /// </summary>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> EditProduct([FromBody] ProductEdit product)
        {
            var updated = _data.UpdateProduct(product);
            return updated is null ? NotFound("Produkt nie znaleziony") : Ok(updated);
        }

        /// <summary>
        /// Usuwa produkt po Id.
        /// </summary>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int Id)
        {
            return _data.DeleteProduct(Id) ? NoContent() : NotFound("Produkt nie znaleziony");
        }

        /// <summary>
        /// Filtruje produkty po tagach (nazwa, opis).
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductResponse>> FilterProducts([FromQuery] string[]? tags)
        {
            var result = _data.FilterProducts(tags);
            return Ok(result);
        }

        /// <summary>
        /// Pobiera pojedynczy produkt po Id.
        /// </summary>
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> GetProductById(int Id)
        {
            var product = _data.GetProducts().FirstOrDefault(p => p.Id == Id);
            return product is null ? NotFound("Produkt nie znaleziony") : Ok(product);
        }

        /// <summary>
        /// Zmniejsza stan magazynu i zwieksza sprzedaz.
        /// </summary>
        [HttpGet("{Id}/countdown")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> CountDown(int Id, int count)
        {
            var updated = _data.ReduceProductVolume(Id, count, out var insufficientStock);
            if (updated == null)
            {
                return NotFound("Produkt nie znaleziony");
            }

            if (insufficientStock)
            {
                return BadRequest("Insufficient stock");
            }

            return Ok(updated);
        }

        /// <summary>
        /// Zwieksza stan magazynu produktu.
        /// </summary>
        [HttpGet("{Id}/countup")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> CountUp(int Id, int count)
        {
            var updated = _data.IncreaseProductVolume(Id, count);
            return updated is null ? NotFound("Produkt nie znaleziony") : Ok(updated);
        }

        /// <summary>
        /// Zwraca liste wszystkich produktow.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            return Ok(_data.GetProducts());
        }
    }
}
