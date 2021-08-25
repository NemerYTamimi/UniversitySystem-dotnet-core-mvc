using System.Collections.Generic;
using System.Linq;
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
            if (_db.Departments.Count(d => d.DeptCode == DeptCode) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsDepartmentNameExist(string DeptName)
        {
            if (_db.Departments.Count(d => d.DeptName == DeptName) > 0)
            {
                return true;
            }
            return false;
        }

        public string SaveDepartment(Department department)
        {
            if (department != null)
            {
                _db.Departments.Add(department);
                _db.SaveChanges();
                return $"Department Successfully created with code = {department.DeptCode}";
            }
            return "Failed to create the department, you enter an empty data";
        }
        public string DeleteDepartment(int? id)
        {
            Department department = _db.Departments.Find(id);
            if (department != null)
            {
                _db.Departments.Remove(department);
                _db.SaveChanges();
                return $"Department Successfully removed";
            }
            return "Failed to remove the department, you enter a wrong data";
        }

        public string EditDepartment(Department department)
        {
            if (department != null)
            {
                _db.Entry(department);
                _db.SaveChanges();
                return $"Department Successfully edited with code = {department.DeptCode}";
            }
            return "Failed to edit the department, you enter an empty data";
        }
        public Department GetDepartment(int? id)
        {
            if (id != null)
            {
                Department department = _db.Departments.Find(id);
                if (department != null)
                {
                    return department;
                }
            }
            return null;
        }
        //public void disposeDb()
        //{
        //    _db.Dispose();
        //}

    }
}
