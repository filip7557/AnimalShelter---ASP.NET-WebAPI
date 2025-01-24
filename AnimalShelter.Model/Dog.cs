namespace AnimalShelter.Models
{
    public class Dog
    {
        public Dog()
        {
        }

        public Dog(string name, int age, Guid breedId)
        {
            Name = name;
            Age = age;
            BreedId = breedId;
        }

        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid? BreedId { get; set; }
        public Breed? Breed { get; set; }
        public int Age { get; set; }
    }
}
