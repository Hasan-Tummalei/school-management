using System.Collections.Generic;

namespace CourseManagement.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
        public User Teacher { get; set; } // Navigation property
        public ICollection<StudentCourse>? StudentCourses { get; set; }
    }
}
