using AnimalShelter.Models;
using AnimalShelter.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnimalShelter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [LogActionFilter]
    public class DogController : ControllerBase
    {
        // GET: api/<DogController>
        [HttpGet]
        public IActionResult Get([FromQuery]string? name = null, [FromQuery]int? age = null, [FromQuery]string? breed = null)
        {
            var service = new DogService();
            var dogs = service.GetAll(name, age, breed);

            if (dogs == null)
                return BadRequest();

            return Ok(dogs);
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var service = new DogService();
            var dog = service.GetById(id);

            if (dog == null)
                return BadRequest();

            return Ok(dog);
        }

        // POST api/<DogController>
        [HttpPost]
        public IActionResult Post([FromBody] Dog dog)
        {
            if (dog == null)
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "Invalid data."
                });

            var service = new DogService();
            var success = service.Save(dog);

            if (!success)
                return BadRequest();

            return Ok("Saved.");
        }

        // PUT api/<DogController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Dog dog)
        {
            if (dog == null)
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "Invalid data."
                });

            var service = new DogService();
            var success = service.Update(id, dog);

            if (!success)
                return BadRequest();

            return Ok("Updated.");
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var service = new DogService();
            var success = service.Delete(id);

            if (!success)
                return BadRequest();

            return Ok("Deleted.");
        }
    }
}
