﻿using AnimalShelter.Common;
using AnimalShelter.Models;
using AnimalShelter.Service.Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnimalShelter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [LogActionFilter]
    public class DogController : ControllerBase
    {
        private IDogService _service;

        public DogController(IDogService dogService)
        {
            _service = dogService;
        }

        // GET: api/<DogController>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]string? name = null, [FromQuery]int? age = null, [FromQuery]string? breed = null)
        {
            var dogFilter = new DogFilter(name, age, breed);
            var dogs = await _service.GetAllAsync(dogFilter);

            if (dogs == null)
                return BadRequest();

            return Ok(dogs);
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var dog = await _service.GetByIdAsync(id);

            if (dog == null)
                return BadRequest();

            return Ok(dog);
        }

        // POST api/<DogController>
        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] Dog dog)
        {
            if (dog == null)
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "Invalid data."
                });

            var success = await _service.SaveAsync(dog);

            if (!success)
                return BadRequest();

            return Ok("Saved.");
        }

        // PUT api/<DogController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Dog dog)
        {
            if (dog == null)
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "Invalid data."
                });

            var success = await _service.UpdateAsync(id, dog);

            if (!success)
                return BadRequest();

            return Ok("Updated.");
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return BadRequest();

            return Ok("Deleted.");
        }
    }
}
