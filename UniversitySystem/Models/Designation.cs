using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversitySystem.Models
{
    public class Designation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Valid Name")]
        public string Name { get; set; }
    }
}