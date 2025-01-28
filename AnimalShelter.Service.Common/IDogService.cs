using AnimalShelter.Common;
using AnimalShelter.Models;

namespace AnimalShelter.Service.Common
{
    public interface IDogService
    {
        public Task<List<Dog>?> GetAllAsync(DogFilter dogFilter, Sorting sorting);
        public Task<Dog?> GetByIdAsync(Guid id);
        public Task<bool> SaveAsync(Dog dog);
        public Task<bool> UpdateAsync(Guid id, Dog dog);
        public Task<bool> DeleteAsync(Guid id);
    }
}
