using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UniversitySystem.Models.ViewModels
{
    public class StudentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Student Id")]
        public string StudentRegNo { get; set; }
    }
}
