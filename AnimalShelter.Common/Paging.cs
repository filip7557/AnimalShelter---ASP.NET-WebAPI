using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelter.Common
{
    public class Paging
    {
        public Paging(int rpp, int pageNumber)
        {
            Rpp = rpp;
            PageNumber = pageNumber;
        }
        public int Rpp { get; set; }
        public int PageNumber { get; set; }
    }
}
