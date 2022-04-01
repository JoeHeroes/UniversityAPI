using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniAPI.Models
{
    public class UpdateStudentDto
    {
      
        [MaxLength(25)]
        public string Title { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public int YearOfStudies { get; set; }
    
    }
}
