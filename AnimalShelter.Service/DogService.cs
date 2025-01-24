using AnimalShelter.Models;
using AnimalShelter.Repository;
using AnimalShelter.Service.Common;

namespace AnimalShelter.Service
{
    public class DogService : IDogService
    {
        public bool Save(Dog dog)
        {
            var repository = new DogRepository();
            return repository.Save(dog);
        }

        public bool Delete(Guid id)
        {
            var repository = new DogRepository();
            return repository.Delete(id);
        }

        public List<Dog>? GetAll(string? filterName, int? filterAge, string? filterBreed)
        {
            var repository = new DogRepository();
            return repository.GetAll(filterName, filterAge, filterBreed);
        }

        public Dog? GetById(Guid id)
        {
            var repository = new DogRepository();
            return repository.GetById(id);
        }

        public bool Update(Guid id, Dog dog)
        {
            var repository = new DogRepository();
            return repository.Update(id, dog);
        }
    }
}
