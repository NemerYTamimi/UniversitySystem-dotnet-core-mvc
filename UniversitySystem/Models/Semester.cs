using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class Semester
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a valid Semester Name")]
        public string Name { get; set; }
    }
}