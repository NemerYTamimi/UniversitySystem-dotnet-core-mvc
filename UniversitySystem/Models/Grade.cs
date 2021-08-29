using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversitySystem.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a valid Room Name")]
        public string Name { get; set; }
    }
}