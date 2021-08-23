using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversitySystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Teacher Name") ]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Teacher Address")]
        [DisplayName("Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter Valid Email Address")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email! Must Be Enter Valid Email Address.")]
        [Remote("IsEmailExists", "Teacher", ErrorMessage = "Email Address already exists!")]
        public string TeacherEmail { get; set; }

        [Required(ErrorMessage = "Enter a Valid Phone Number")]
        [DisplayName("Phone Number")]
        [Phone]
        [StringLength(14, MinimumLength = 7, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Select A Designation" )]
        [DisplayName("Designation")]
        public int DesignationId { get; set; }
        public virtual Designation Designation { get; set; }

        [Required(ErrorMessage = "Select Department")]
        [DisplayName ("Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Negative Credit Not Allow ! Enter Positive Credit Number")]
        [DisplayName ("Credit to be Taken")]
        public double CreditTaken { get; set; }

        public double CreditLeft { get; set; }

        public string LoginUserId { get; set; }

        public static string Role = Utility.Helper.Teacher;
    }
}