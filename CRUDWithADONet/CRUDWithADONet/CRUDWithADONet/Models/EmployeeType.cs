using System.ComponentModel.DataAnnotations;

namespace CRUDWithADONet.Models
{
    public class EmployeeType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
