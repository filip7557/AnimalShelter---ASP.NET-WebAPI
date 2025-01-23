using AnimalShelter.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnimalShelter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [LogActionFilter]
    public class DogController : ControllerBase
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Database=dogs;Username=postgres;Password=test";

        // GET: api/<DogController>
        [HttpGet]
        public IActionResult Get([FromQuery]string? name = null, [FromQuery]int? age = null, [FromQuery]string? breed = null)
        {
            var dogs = new List<Dog>();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var commandText = "SELECT \"Dog\".\"Id\", \"Dog\".\"Name\", \"Age\", \"Breed\".\"Name\", \"Breed\".\"Id\" FROM \"Dog\" LEFT JOIN \"Breed\" ON \"Dog\".\"BreedId\" = \"Breed\".\"Id\" WHERE 1 = 1";
                    using var command = new NpgsqlCommand(commandText, connection);
                    if (name != null)
                    {
                        command.CommandText += " AND \"Dog.Name\".\"Name\" = @name";
                        command.Parameters.AddWithValue("name", name);
                    }
                    if (age != null)
                    {
                        command.CommandText += " AND \"Age\" = @age";
                        command.Parameters.AddWithValue("age", age);
                    }
                    if (breed != null)
                    {
                        command.CommandText += " AND \"Breed\".\"Name\" = @breed";
                        command.Parameters.AddWithValue("breed", breed);
                    }

                    connection.Open();

                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var dog = new Dog()
                            { 
                                Name = reader[1].ToString()!,
                                Age = int.TryParse(reader[2].ToString(), out int result) ? result : 0,
                                Id = Guid.Parse(reader[0].ToString()!)
                            };
                            dog.BreedId = Guid.Parse(reader[4].ToString()!);
                            dog.Breed = new Breed()
                            {
                                Name = reader[3].ToString()!,
                                Id = (Guid)dog.BreedId
                            };
                            dogs.Add(dog);
                        }
                    }
                    else
                    {
                        connection.Close();
                        return NotFound();
                    }

                    connection.Close();

                    return Ok(dogs);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = ex.Message
                });
            }
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var dog = new Dog() { Name = "" };
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var commandText = "SELECT \"Dog\".\"Id\", \"Dog\".\"Name\", \"Age\", \"Breed\".\"Name\", \"Breed\".\"Id\" FROM \"Dog\" LEFT JOIN \"Breed\" ON \"Dog\".\"BreedId\" = \"Breed\".\"Id\" WHERE \"Dog\".\"Id\" = @id;";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        dog.Id = Guid.Parse(reader[0].ToString()!);
                        dog.Name = reader[1].ToString()!;
                        dog.Age = int.TryParse(reader[2].ToString(), out int result) ? result : 0;
                        dog.BreedId = Guid.Parse(reader[4].ToString()!);
                        dog.Breed = new Breed()
                        {
                            Name = reader[3].ToString()!,
                            Id = (Guid)dog.BreedId
                        };
                    } else
                    {
                        connection.Close();
                        return NotFound();
                    }

                    connection.Close();

                    return Ok(dog);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = ex.Message
                });
            }
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

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var commandText = "INSERT INTO \"Dog\" (\"Id\", \"Name\", \"Age\", \"BreedId\") values (@id, @name, @age, @breedid);";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                    command.Parameters.AddWithValue("name", dog.Name);
                    command.Parameters.AddWithValue("age", dog.Age);
                    command.Parameters.AddWithValue("breedid", dog.BreedId);

                    connection.Open();
                    
                    var affectedRows = command.ExecuteNonQuery();
                    if (affectedRows == 0)
                        return BadRequest();
                    
                    connection.Close();

                    return Ok("Saved.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = ex.Message
                });
            }
        }

        // PUT api/<DogController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Dog dog)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var commandText = "SELECT \"Id\", \"Name\", \"Age\" FROM \"Dog\" WHERE \"Id\" = @id;";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        connection.Close();
                        return NotFound();
                    }

                    connection.Close();

                    connection.Open();

                    commandText = "UPDATE \"Dog\" set \"Name\" = @name, \"Age\" = @age WHERE \"Id\" = @id;";
                    using var updateCommand = new NpgsqlCommand(commandText, connection);
                    updateCommand.Parameters.AddWithValue("name", dog.Name);
                    updateCommand.Parameters.AddWithValue("age", dog.Age);
                    updateCommand.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    var affectedRows = updateCommand.ExecuteNonQuery();
                    if (affectedRows == 0)
                    {
                        connection.Close();
                        return BadRequest();
                    }

                    connection.Close();

                    return Ok("Updated.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = ex.Message
                });
            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var commandText = "DELETE FROM \"Dog\" WHERE \"Id\" = @id;";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var affectedRows = command.ExecuteNonQuery();
                    if (affectedRows == 0)
                    {
                        connection.Close();
                        return NotFound();
                    }

                    connection.Close();

                    return Ok("Deleted.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = ex.Message
                });
            }
        }
    }
}
