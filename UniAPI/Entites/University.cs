using System.Collections.Generic;
using UniAPI.Authorization;

namespace UniAPI.Entites
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public virtual Teacher Rektor { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public int? CreateById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual List<Department> Departments { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }


    }
}
