using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a valid Room Name")]
        public string Name { get; set; }
    }
}