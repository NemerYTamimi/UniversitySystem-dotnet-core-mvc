using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models.ViewModels
{
    public class TeacherVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }
    }
}
