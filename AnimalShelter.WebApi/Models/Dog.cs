namespace AnimalShelter.WebApi.Models
{
    public class Dog
    {

        public Dog(string name, int age, int id)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
