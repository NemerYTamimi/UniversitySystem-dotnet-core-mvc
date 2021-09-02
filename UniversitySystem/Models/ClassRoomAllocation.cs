using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class ClassRoomAllocation
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        [Display(Name = "Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Required]
        [Display(Name = "Day")]
        public int DayId { get; set; }
        public virtual Day Day { get; set; }

        [Required]
        public double StartTime { get; set; }
        [Required]
        public double EndTime { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string RoomStatus { get; set; }
    }
}