namespace AnimalShelter.WebApi.Models
{
    public class Dog
    {

        public Dog(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
