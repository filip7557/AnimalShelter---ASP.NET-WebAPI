namespace AnimalShelter.WebApi.Models
{
    public class Breed
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}