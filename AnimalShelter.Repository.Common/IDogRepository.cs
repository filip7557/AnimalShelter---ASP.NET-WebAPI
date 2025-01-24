using AnimalShelter.Models;
using System.Runtime.CompilerServices;

namespace AnimalShelter.Repository.Common
{
    public interface IDogRepository
    {
        public List<Dog>? GetAll(string? filterName, int? filterAge, string? filterBreed);
        public Dog? GetById(Guid id);
        public bool Save(Dog dog);
        public bool Update(Guid id, Dog dog);
        public bool Delete(Guid id);
    }
}
