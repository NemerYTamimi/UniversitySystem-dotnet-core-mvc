using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;

namespace UniversitySystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _db;
        public DepartmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Department> GetAllDepartment()
        {
            return _db.Departments.ToList();
        }

        public bool IsDepartmentCodeExist(string DeptCode)
        {
            throw new NotImplementedException();
        }

        public bool IsDepartmentNameExist(string DeptName)
        {
            throw new NotImplementedException();
        }

        public string SaveDepartment(Department department)
        {
            if(department != null)
            {
                _db.Departments.Add(department);
                _db.SaveChanges();
                return $"Department Successfully created with code = {department.DeptCode}";
            }
            return "Failed to create the department, you enter an empty data";
        }
    }
}
