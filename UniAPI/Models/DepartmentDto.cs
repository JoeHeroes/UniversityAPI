using System.ComponentModel.DataAnnotations;

namespace UniAPI.Models
{
    public class DepartmentDto
    {
      
        public string Name { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        
    }
}
