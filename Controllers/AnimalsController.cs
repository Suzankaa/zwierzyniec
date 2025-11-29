using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;

namespace Zwierzyniec.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AnimalsController : ControllerBase
    {
        [HttpPut("add")]
        public IActionResult AddAnimal(AnimalAdd animal)
        {
            return Ok(animal);
        }

        [HttpPut("edit")]
        public IActionResult EditAnimal(AnimalEdit animal)
        {
            return Ok(animal);
        }

        [HttpDelete("delete")]
        public IActionResult DeleteAnimal(int Id)
        {
            return Ok();
        }

        [HttpGet("filter")]
        public IActionResult FilterAnimals(string[] tags)
        {
            return Ok();
        }

        [HttpGet("{Id}")]
        public IActionResult GetAnimalById(int Id)
        {
            return Ok();
        }
    }
}
