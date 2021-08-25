using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class Day
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a valid Day Name")]
        public string Name { get; set; }
    }
}