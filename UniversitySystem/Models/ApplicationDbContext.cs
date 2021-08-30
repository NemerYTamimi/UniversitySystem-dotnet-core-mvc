using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models.ViewModels;

namespace UniversitySystem.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Day> Days { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<CourseAssign> CourseAssigns { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ClassRoomAllocation> ClassRoomAllocations { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<EnrollCourse> EnrollCourses { get; set; }
        public DbSet<Finanial> Finanials { get; set; }
        public DbSet<Parent> Parents { get; set; }


    }
}
