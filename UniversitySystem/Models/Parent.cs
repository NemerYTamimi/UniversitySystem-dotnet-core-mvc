using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace UniversitySystem.Models
{
    public class Parent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Parent Name")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Valid Email Address")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email! Must Be Enter Valid Email Address.")]
        [Remote("IsEmailExists", "Parent", ErrorMessage = "Email Address already exists!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a Valid Phone Number")]
        [DisplayName("Phone Number")]
        [Phone]
        [StringLength(14, MinimumLength = 7, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
