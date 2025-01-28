using AnimalShelter.Common;
using AnimalShelter.Models;
using AnimalShelter.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;

namespace AnimalShelter.Repository
{
    public class DogRepository : IDogRepository
    {
        private string _connectionString;
        public DogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> SaveAsync(Dog dog)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var commandText = "INSERT INTO \"Dog\" (\"Id\", \"Name\", \"Age\", \"BreedId\") values (@id, @name, @age, @breedid);";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                    command.Parameters.AddWithValue("name", dog.Name);
                    command.Parameters.AddWithValue("age", dog.Age);
                    command.Parameters.AddWithValue("breedid", dog.BreedId);

                    connection.Open();

                    var affectedRows = await command.ExecuteNonQueryAsync();
                    if (affectedRows == 0)
                        return false;

                    connection.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var commandText = "SELECT \"Id\", \"Name\", \"Age\" FROM \"Dog\" WHERE \"Id\" = @id;";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var reader = await command.ExecuteReaderAsync();

                    if (!reader.HasRows)
                    {
                        connection.Close();
                        return false;
                    }

                    connection.Close();

                    connection.Open();

                    commandText = "DELETE FROM \"Dog\" WHERE \"Id\" = @id;";
                    using var deleteCommand = new NpgsqlCommand(commandText, connection);
                    deleteCommand.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var affectedRows = await deleteCommand.ExecuteNonQueryAsync();
                    if (affectedRows == 0)
                    {
                        connection.Close();
                        return false;
                    }

                    connection.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> CountAsync()
        {
            int count = 0;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var commandText = "SELECT COUNT(\"Id\") FROM \"Dog\"";
                    using var command = new NpgsqlCommand(commandText, connection);

                    connection.Open();

                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        int.TryParse(reader[0].ToString(), out count);
                    }
                    else
                    {
                        connection.Close();
                        return 0;
                    }
                    connection.Close();

                    return count;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        public async Task<List<Dog>?> GetAllAsync(DogFilter dogFilter, Sorting sorting, Paging paging)
        {
            var dogs = new List<Dog>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    StringBuilder commandText = new StringBuilder("SELECT \"Dog\".\"Id\", \"Dog\".\"Name\", \"Age\", \"Breed\".\"Name\", \"Breed\".\"Id\" FROM \"Dog\" LEFT JOIN \"Breed\" ON \"Dog\".\"BreedId\" = \"Breed\".\"Id\" WHERE 1 = 1");

                    dogFilter.Apply(commandText);
                    sorting.Apply(commandText);
                    paging.Apply(commandText);

                    using var command = new NpgsqlCommand(commandText.ToString(), connection);
                    if (!string.IsNullOrEmpty(dogFilter.Name)) command.Parameters.AddWithValue("name", dogFilter.Name);
                    if (dogFilter.Age != null) command.Parameters.AddWithValue("age", dogFilter.Age);
                    if (!string.IsNullOrEmpty(dogFilter.Breed)) command.Parameters.AddWithValue("breed", dogFilter.Breed);
                    command.Parameters.AddWithValue("rpp", paging.Rpp);
                    command.Parameters.AddWithValue("pageNumber", paging.PageNumber);

                    connection.Open();

                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
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
                        return null;
                    }
                    connection.Close();

                    return dogs;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        

        public async Task<Dog?> GetByIdAsync(Guid id)
        {
            try
            {
                var dog = new Dog() { Name = "" };
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var commandText = "SELECT \"Dog\".\"Id\", \"Dog\".\"Name\", \"Age\", \"Breed\".\"Name\", \"Breed\".\"Id\" FROM \"Dog\" LEFT JOIN \"Breed\" ON \"Dog\".\"BreedId\" = \"Breed\".\"Id\" WHERE \"Dog\".\"Id\" = @id;";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        dog.Id = Guid.Parse(reader[0].ToString()!);
                        dog.Name = reader[1].ToString()!;
                        dog.Age = int.TryParse(reader[2].ToString(), out int result) ? result : 0;
                        dog.BreedId = Guid.Parse(reader[4].ToString()!);
                        dog.Breed = new Breed()
                        {
                            Name = reader[3].ToString()!,
                            Id = (Guid)dog.BreedId
                        };
                    }
                    else
                    {
                        connection.Close();
                        return null;
                    }

                    connection.Close();

                    return dog;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, Dog dog)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var commandText = "SELECT \"Id\", \"Name\", \"Age\" FROM \"Dog\" WHERE \"Id\" = @id;";
                    using var command = new NpgsqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    connection.Open();

                    var reader = await command.ExecuteReaderAsync();

                    if (!reader.HasRows)
                    {
                        connection.Close();
                        return false;
                    }

                    connection.Close();

                    connection.Open();

                    commandText = "UPDATE \"Dog\" set \"Name\" = @name, \"Age\" = @age, \"BreedId\" = @breed WHERE \"Id\" = @id;";
                    using var updateCommand = new NpgsqlCommand(commandText, connection);
                    updateCommand.Parameters.AddWithValue("name", dog.Name);
                    updateCommand.Parameters.AddWithValue("age", dog.Age);
                    updateCommand.Parameters.AddWithValue("breed", dog.BreedId);
                    updateCommand.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                    var affectedRows = await updateCommand.ExecuteNonQueryAsync();
                    if (affectedRows == 0)
                    {
                        connection.Close();
                        return false;
                    }

                    connection.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
