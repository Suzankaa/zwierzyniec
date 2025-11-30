using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zwierzyniec.Models.Input;
using Zwierzyniec.Models.Output;
using Zwierzyniec.Services;

namespace Zwierzyniec.Controllers
{
    /// <summary>
    /// Endpointy dla zwierzat z danymi w pamieci.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class AnimalsController : ControllerBase
    {
        private readonly IMockDataService _data;

        public AnimalsController(IMockDataService data)
        {
            _data = data;
        }

        /// <summary>
        /// Dodaje nowe zwierze.
        /// </summary>
        [HttpPut("add")]
        [ProducesResponseType(typeof(AnimalResponse), StatusCodes.Status201Created)]
        public ActionResult<AnimalResponse> AddAnimal([FromBody] AnimalAdd animal)
        {
            var created = _data.AddAnimal(animal);
            Response.Headers["X-Message"] = "Zwierze dodane";
            return CreatedAtAction(nameof(GetAnimalById), new { Id = created.Id }, created);
        }

        /// <summary>
        /// Aktualizuje istniejace zwierze po Id.
        /// </summary>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(AnimalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AnimalResponse> EditAnimal([FromBody] AnimalEdit animal)
        {
            var updated = _data.UpdateAnimal(animal);
            return updated is null ? NotFound("Zwierze nie znalezione") : Ok(updated);
        }

        /// <summary>
        /// Usuwa zwierze po Id.
        /// </summary>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteAnimal(int Id)
        {
            return _data.DeleteAnimal(Id) ? NoContent() : NotFound("Zwierze nie znalezione");
        }

        /// <summary>
        /// Filtruje zwierzeta po tagach (nazwa, opis, gatunek, status).
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<AnimalResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AnimalResponse>> FilterAnimals([FromQuery] string[]? tags)
        {
            var result = _data.FilterAnimals(tags);
            return Ok(result);
        }

        /// <summary>
        /// Pobiera pojedyncze zwierze po Id.
        /// </summary>
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(AnimalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AnimalResponse> GetAnimalById(int Id)
        {
            var animal = _data.GetAnimalById(Id);
            return animal is null ? NotFound("Zwierze nie znalezione") : Ok(animal);
        }

        /// <summary>
        /// Zwraca liste wszystkich zwierzat.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AnimalResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AnimalResponse>> GetAnimals()
        {
            return Ok(_data.GetAnimals());
        }
    }
}
