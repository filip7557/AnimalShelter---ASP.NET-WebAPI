using AnimalShelter.Models;
using AnimalShelter.Repository;
using AnimalShelter.Service.Common;

namespace AnimalShelter.Service
{
    public class DogService : IDogService
    {
        public async Task<bool> SaveAsync(Dog dog)
        {
            var repository = new DogRepository();
            return await repository.SaveAsync(dog);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var repository = new DogRepository();
            return await repository.DeleteAsync(id);
        }

        public async Task<List<Dog>?> GetAllAsync(string? filterName, int? filterAge, string? filterBreed)
        {
            var repository = new DogRepository();
            return await repository.GetAllAsync(filterName, filterAge, filterBreed);
        }

        public async Task<Dog?> GetByIdAsync(Guid id)
        {
            var repository = new DogRepository();
            return await repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, Dog dog)
        {
            var repository = new DogRepository();
            return await repository.UpdateAsync(id, dog);
        }
    }
}
