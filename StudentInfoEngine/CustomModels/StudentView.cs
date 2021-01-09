using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentInfoEngine.CustomModels
{
    public class StudentView
    {
        public int ID { get; set; }
        public string PrivateNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public string GenderDesc { get; set; }
    }
}
