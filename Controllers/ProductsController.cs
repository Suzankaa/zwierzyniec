using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;

namespace Zwierzyniec.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ProductsController : ControllerBase
    {
        [HttpPut("add")]
        public IActionResult AddProduct(AnimalAdd animal)
        {
            return Ok(animal);
        }

        [HttpPut("edit")]
        public IActionResult EditProduct(AnimalAdd animal)
        {
            return Ok(animal);
        }

        [HttpDelete("delete")]
        public IActionResult DeleteProduct(int Id)
        {
            return Ok();
        }

        [HttpGet("filter")]
        public IActionResult FilterProducts(string[] tags)
        {
            return Ok();
        }

        [HttpGet("{Id}")]
        public IActionResult CountDown(int Id, int count)
        {
            return Ok();
        }

        [HttpGet("countup/{Id}")]
        public IActionResult CountUp(int Id, int count)
        {
            return Ok();
        }
    }
}
