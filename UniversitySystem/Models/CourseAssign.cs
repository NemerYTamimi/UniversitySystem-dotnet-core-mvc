using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class CourseAssign
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please Select Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required(ErrorMessage = "Please Select Teacher")]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}