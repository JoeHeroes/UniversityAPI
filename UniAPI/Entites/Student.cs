using System;

namespace UniAPI.Entites
{
    public class Student
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int IndexNumber { get; set; }
        public string PESEL { get; set; }
        public string Specialization { get; set; }
        public int YearOfStudies { get; set; }
        public virtual Address Address { get; set; }


    }
}
