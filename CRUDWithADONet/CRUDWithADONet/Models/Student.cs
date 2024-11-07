using System.ComponentModel.DataAnnotations;

namespace CRUDWithADONet.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string City { get; set; }

    }
}
