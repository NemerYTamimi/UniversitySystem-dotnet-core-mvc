using System.Collections.Generic;
using UniversitySystem.Models;

namespace UniversitySystem.Services
{
    public interface IStudentService
    {
        public bool AddCredit(string StudentRegNo, double credit);
        public bool UseCredit(string StudentRegNo, double credit);
        public double CheckCredit(string StudentRegNo);
        public int NumOfEnrolledCourses(string StudentRegNo, int SemesterId);
        public bool IsEnrolled(string studentRegNo, int semesterId, int courseId);
        public bool HasBrotherAndOlder(string studentRegNo);
        public List<EnrollCourse> StudentCourses(string studentRegNo, int semesterId);
    }
}
