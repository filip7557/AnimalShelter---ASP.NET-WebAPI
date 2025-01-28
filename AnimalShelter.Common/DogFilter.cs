using System.Text;

namespace AnimalShelter.Common
{
    public class DogFilter
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Breed { get; set; }

        public void Apply(StringBuilder command)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                command.Append(" AND \"Dog\".\"Name\" = @name");
            }
            if (Age != null)
            {
                command.Append(" AND \"Age\" = @age");
            }
            if (!string.IsNullOrEmpty(Breed))
            {
                command.Append(" AND \"Breed\".\"Name\" = @breed");
            }
        }
    }
}
