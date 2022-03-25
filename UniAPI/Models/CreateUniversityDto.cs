using System.ComponentModel.DataAnnotations;

namespace UniAPI.Models
{
    public class CreateUniversityDto
    {
        /*
        [CreditCard]
        [Compare("OtherProperty")]
        [EmailAddress]
        [Phone]
        [Range(int.MinValue,int.MaxValue)]
        [RegularExpression()]
        */



        [Required]
        [MaxLength(25)]
       
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }


    }
}
