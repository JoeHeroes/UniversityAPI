using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniAPI.Models
{
    public class CreateStudentDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int IndexNumber { get; set; }
        public string PESEL { get; set; }
        public string Specialization { get; set; }
        public int YearOfStudies { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}

