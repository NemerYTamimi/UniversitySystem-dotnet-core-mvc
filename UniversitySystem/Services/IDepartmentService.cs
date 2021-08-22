using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;

namespace UniversitySystem.Services
{
    public interface IDepartmentService
    {
        public string SaveDepartment(Department department);
        public List<Department> GetAllDepartment();
        public bool IsDepartmentCodeExist(string DeptCode);
        public bool IsDepartmentNameExist(string DeptName);
    }
}
