using AnimalShelter.Models;
using System.Runtime.CompilerServices;

namespace AnimalShelter.Repository.Common
{
    public interface IDogRepository
    {
        public Task<List<Dog>?> GetAllAsync(string? filterName, int? filterAge, string? filterBreed);
        public Task<Dog?> GetByIdAsync(Guid id);
        public Task<bool> SaveAsync(Dog dog);
        public Task<bool> UpdateAsync(Guid id, Dog dog);
        public Task<bool> DeleteAsync(Guid id);
    }
}
