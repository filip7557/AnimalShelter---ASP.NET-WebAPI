using AnimalShelter.Common;
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

        public async Task<PagedResponse<Dog>> GetAllAsync(DogFilter dogFilter, Sorting sorting, Paging paging)
        {
            var dogs = await _dogRepository.GetAllAsync(dogFilter, sorting, paging);
            var count = await CountAsync(dogFilter);
            return new PagedResponse<Dog>()
            {
                Data = dogs ?? new List<Dog>(),
                PageNumber = paging.PageNumber,
                PageSize = paging.Rpp,
                TotalRecords = count
            };
        }

        public async Task<Dog?> GetByIdAsync(Guid id)
        {
            return await _dogRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, Dog dog)
        {
            return await _dogRepository.UpdateAsync(id, dog);
        }

        public async Task<int> CountAsync(DogFilter dogFilter)
        {
            return await _dogRepository.CountAsync(dogFilter);
        }
    }
}