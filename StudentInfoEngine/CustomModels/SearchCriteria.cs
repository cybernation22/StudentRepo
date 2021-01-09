using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInfoEngine.Models
{
    public class SearchCriteria
    {

        public int PageSize { get; set; } = 5;
        public int Page { get; set; } = 0;  

        public string PrivateNumber { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
    }
}
