using System.Text;


namespace AnimalShelter.Common
{
    public class Sorting
    {
        public required string OrderBy { get; set; }
        public required string SortOrder { get; set; }

        public void Apply(StringBuilder command)
        {
            command.Append($" ORDER BY \"Dog\".\"{OrderBy}\" {SortOrder}");
        }
    }
}
