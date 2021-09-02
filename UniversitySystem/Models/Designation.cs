using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class Designation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Valid Designation Name")]
        public string Name { get; set; }
    }
}