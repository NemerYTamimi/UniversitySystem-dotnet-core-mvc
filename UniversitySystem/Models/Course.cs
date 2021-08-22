using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UniversitySystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Unique Course Code")]
        [Display(Name = "Code")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Course Code Must be 5 characters long")]
        [Remote("IsUniqueCode", "Course", ErrorMessage = "Course Code already exists!")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Please Enter Course Name")]
        [Display(Name = "Name")]
        [Remote("IsUniqueName", "Course", ErrorMessage = "Course Name already exists!")]
        [StringLength(150)]
        [Column(TypeName = "Varchar")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Enter Course Cradit")]
        [Display(Name = "Credit")]
        [Range(0.5, 5.0, ErrorMessage = "Credit must be within 0.5 to 5.0")]
        public double CourseCredit { get; set; }

        [Required(ErrorMessage = "Enter Full Description This Course ")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Column(TypeName = "Varchar")]
        public string CourseDescription { get; set; }

       
        public string CourseAssignTo { get; set; }

      
        public bool CourseStatus { get; set; }
        [Required(ErrorMessage = "Select Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        [Required(ErrorMessage = "Select Semester")]
        [Display(Name = "Semester")]
        public int SemesterId { get; set; }

        [ForeignKey("SemesterId")]
        public virtual Semester Semester { get; set; }
    }
}