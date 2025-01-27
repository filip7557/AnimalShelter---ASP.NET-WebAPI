using AnimalShelter.Models;
using AnimalShelter.Repository.Common;
using AnimalShelter.Service.Common;

namespace AnimalShelter.Service
{
    public class DogService : IDogService
    {
        private IDogRepository _dogRepository;

        public DogService(IDogRepository repository)
        {
            _dogRepository = repository;
        }

        public async Task<bool> SaveAsync(Dog dog)
        {
            return await _dogRepository.SaveAsync(dog);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _dogRepository.DeleteAsync(id);
        }

        public async Task<List<Dog>?> GetAllAsync(string? filterName, int? filterAge, string? filterBreed)
        {
            return await _dogRepository.GetAllAsync(filterName, filterAge, filterBreed);
        }

        public async Task<Dog?> GetByIdAsync(Guid id)
        {
            return await _dogRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, Dog dog)
        {
            return await _dogRepository.UpdateAsync(id, dog);
        }
    }
}
