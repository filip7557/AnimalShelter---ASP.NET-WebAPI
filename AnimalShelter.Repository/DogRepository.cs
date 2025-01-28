using AnimalShelter.Common;
using AnimalShelter.Models;
using AnimalShelter.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

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
                    //TODO: use string builder
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

        public async Task<List<Dog>?> GetAllAsync(DogFilter dogFilter, Sorting sorting, Paging paging)
        {
            var dogs = new List<Dog>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var commandText = "SELECT \"Dog\".\"Id\", \"Dog\".\"Name\", \"Age\", \"Breed\".\"Name\", \"Breed\".\"Id\" FROM \"Dog\" LEFT JOIN \"Breed\" ON \"Dog\".\"BreedId\" = \"Breed\".\"Id\" WHERE 1 = 1";
                    using var command = new NpgsqlCommand(commandText, connection);

                    AddDogFilter(dogFilter, command);
                    AddSorting(sorting, command);
                    AddPaging(paging, command);

                    Console.WriteLine(command.CommandText);

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
            catch (Exception)
            {
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

        private void AddDogFilter(DogFilter dogFilter, NpgsqlCommand command)
        {
            if (dogFilter.Name != null)
            {
                command.CommandText += " AND \"Dog\".\"Name\" = @name";
                command.Parameters.AddWithValue("name", dogFilter.Name);
            }
            if (dogFilter.Age != null)
            {
                command.CommandText += " AND \"Age\" = @age";
                command.Parameters.AddWithValue("age", dogFilter.Age);
            }
            if (dogFilter.Breed != null)
            {
                command.CommandText += " AND \"Breed\".\"Name\" = @breed";
                command.Parameters.AddWithValue("breed", dogFilter.Breed);
            }
        }

        private void AddSorting(Sorting sorting, NpgsqlCommand command)
        {
            command.CommandText += $" ORDER BY \"Dog\".\"{sorting.OrderBy}\" {sorting.SortOrder}";
        }
        private void AddPaging(Paging paging, NpgsqlCommand command)
        {
            command.CommandText += $" LIMIT {paging.Rpp} OFFSET ({paging.PageNumber} - 1) * {paging.Rpp}";
        }
    }
}
