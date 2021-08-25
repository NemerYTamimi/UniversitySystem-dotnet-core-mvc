using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a Valid Room Name")]
        public string Name { get; set; }
    }
}