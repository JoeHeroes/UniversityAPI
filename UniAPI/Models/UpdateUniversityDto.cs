using System.ComponentModel.DataAnnotations;

namespace UniAPI.Models
{
    public class UpdateUniversityDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactEmail { get; set; }
    }
}
