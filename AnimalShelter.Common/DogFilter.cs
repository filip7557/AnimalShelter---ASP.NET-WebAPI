namespace AnimalShelter.Common
{
    public class DogFilter
    {
        public DogFilter(string? name, int? age, string? breed)
        {
            Name = name;
            Age = age;
            Breed = breed;
        }

        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Breed { get; set; }
    }
}
