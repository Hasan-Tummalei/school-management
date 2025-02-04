using CourseManagement.Data;
using CourseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Course
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.Include(c => c.Teacher).ToListAsync();
            return View(courses);
        }

        // GET: /Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Course/Create
        [HttpPost]
        public async Task<IActionResult> Create(string title, string description)
        {
            if (HttpContext.Session.GetString("UserRole") != "Teacher")
            {
                return Unauthorized();
            }

            var teacherId = int.Parse(HttpContext.Session.GetString("UserId"));

            var course = new Course
            {
                Title = title,
                Description = description,
                TeacherId = teacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Course/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            return View(course);
        }

        // POST: /Course/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string title, string description)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (HttpContext.Session.GetString("UserId") != course.TeacherId.ToString())
            {
                return Unauthorized();
            }

            course.Title = title;
            course.Description = description;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // POST: /Course/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (HttpContext.Session.GetString("UserId") != course.TeacherId.ToString())
            {
                return Unauthorized();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
