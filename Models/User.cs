using System.Collections.Generic;

namespace CourseManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Store hashed password
        public UserRole Role { get; set; } // Teacher or Student

        public ICollection<StudentCourse>? StudentCourses { get; set; } // If user is a student
        public ICollection<Course>? Courses { get; set; } // If user is a teacher
    }

    public enum UserRole
    {
        Teacher,
        Student
    }
}
