using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniAPI.Entites
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        public int UniversityId { get; set; }
        public virtual Department Departments { get; set; }
        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
