using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversitySystem.Models
{
    public class CourseAssign
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please Select Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required (ErrorMessage = "Please Select Teacher")]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}