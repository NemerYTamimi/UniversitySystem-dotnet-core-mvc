using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversitySystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required ]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email format is Incorrect.")]
        [Remote("IsEmailExists", "Teacher", ErrorMessage = "Email already exists!", AdditionalFields = "Id")]
        public string Email { set; get; }

        [Required(ErrorMessage = "The Field is required.")]
        [Phone(ErrorMessage = "phone number format is Incorrect.")]
        [StringLength(14, MinimumLength = 7, ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Date Field is Required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } 

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "The Department Field is required.")]
        [DisplayName("Department")]

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [DisplayName("Student Id")]
        public string StudentRegNo { get; set; } // Auto Generated

        [Required(ErrorMessage = "The Parent Field is required.")]
        [DisplayName("Parent")]
        public int ParentId { get; set; }
        public Parent Parent { get; set; }
        public string LoginUserId { get; set; }

        public static string Role = Utility.Helper.Teacher;
        public bool Status { get; set; }
    }
}