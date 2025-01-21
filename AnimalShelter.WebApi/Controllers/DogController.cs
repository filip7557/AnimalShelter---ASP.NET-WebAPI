using AnimalShelter.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnimalShelter.WebApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        public static List<Dog> dogs = new List<Dog>();

        // GET: api/<DogController>
        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine(dogs.Count);
            if (!dogs.Any())
            {
                return NoContent();
            }
            return Ok(dogs);
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var resource = dogs.FirstOrDefault(p => p.Id == id);

            if (resource == null)
            {
                return NotFound(new
                {
                    error = "Not Found",
                    message = $"The resource with ID {id} does not exist."
                });
            }

            return Ok(resource);
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

            if (dogs.Any(p => p.Id == dog.Id))
                return Conflict(new
                {
                    error = "Conflict",
                    message = $"ID {dog.Id} is already taken."
                });

            dogs.Add(dog);
            Console.WriteLine(dogs.Count);
            return Ok();
        }

        // PUT api/<DogController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Dog dog)
        { 
            var resource = dogs.FirstOrDefault(p => p.Id == id);

            if (resource == null)
            {
                return NotFound(new
                {
                    error = "Not Found",
                    message = $"The resource with ID {id} does not exist."
                });
            }

            if (dog == null || dog.Name == null)
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "Invalid data."
                });

            resource.Name = dog.Name;
            resource.Age = dog.Age;

            return Ok();
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var resource = dogs.FirstOrDefault(p => p.Id == id);

            if (resource == null)
            {
                return NotFound(new
                {
                    error = "Not Found",
                    message = $"The resource with ID {id} does not exist."
                });
            }

            dogs.Remove(resource);

            return Ok();
        }
    }
}
