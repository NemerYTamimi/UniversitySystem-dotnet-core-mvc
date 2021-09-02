using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class Finanial
    {
        public int Id { get; set; }

        [Required]
        [Range(0.0, int.MaxValue)]
        public double Credit { get; set; }

        [Required]
        [Range(0.0, int.MaxValue)]
        public double CreditUsed { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
