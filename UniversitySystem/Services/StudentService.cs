using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;

namespace UniversitySystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _db;

        public StudentService(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool AddCredit(string studentRegNo, double credit)
        {
            Student student = _db.Students.FirstOrDefault(s => s.StudentRegNo == studentRegNo);
            if (student != null)
            {
                Finanial finanial = _db.Finanials.FirstOrDefault(s => s.StudentId == student.Id);
                if (finanial != null)
                {
                    finanial.Credit += credit;
                    _db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public double CheckCredit(string studentRegNo)
        {
            Student student = _db.Students.FirstOrDefault(s => s.StudentRegNo == studentRegNo);
            if (student != null)
            {
                Finanial finanial = _db.Finanials.FirstOrDefault(s => s.StudentId == student.Id);
                if (finanial != null)
                {
                    return finanial.Credit;
                }
            }
            return -1;
        }

        public bool UseCredit(string studentRegNo, double credit)
        {
            Student student = _db.Students.FirstOrDefault(s => s.StudentRegNo == studentRegNo);
            if (student != null)
            {
                Finanial finanial = _db.Finanials.FirstOrDefault(s => s.StudentId == student.Id);
                if (finanial != null)
                {
                    if (HasBrotherAndOlder(studentRegNo))
                    {
                        credit = credit - (credit * 0.15);
                    }
                    if (CheckCredit(studentRegNo) - credit >= 0)
                    {
                        finanial.Credit -= credit;
                        finanial.CreditUsed += credit;
                        _db.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public int NumOfEnrolledCourses(string studentRegNo, int semesterId)
        {
            try
            {
                int count = _db.EnrollCourses.Count(s => (s.RegistrationNo == studentRegNo && s.Course.SemesterId == semesterId));
                return count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool IsEnrolled(string studentRegNo, int semesterId, int courseId)
        {
            try
            {
                return _db.EnrollCourses.Any(s => s.RegistrationNo == studentRegNo && s.CourseId == courseId && s.Course.SemesterId == semesterId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool HasBrotherAndOlder(string studentRegNo)
        {
            Student student = _db.Students.First(s => s.StudentRegNo == studentRegNo);
            if(student != null)
            {
                if (_db.Students.Count(s => s.ParentId == student.ParentId) > 1)
                {
                    foreach (Student brother in _db.Students)
                    {
                        if (brother.Id != student.Id && brother.ParentId == student.ParentId && brother.Date < student.Date)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
