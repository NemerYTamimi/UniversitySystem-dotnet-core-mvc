using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversitySystem.Models
{
    public class Day
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a valid Day Name")]
        public string Name { get; set; }
    }
}