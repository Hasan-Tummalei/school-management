using CourseManagement.Data;
using CourseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagement.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Subscription
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = int.Parse(HttpContext.Session.GetString("UserId"));

            var subscriptions = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Teacher) // Ensure Teacher is loaded
                .ToListAsync();

            return View(subscriptions);
        }


        // POST: /Subscription/Subscribe/{courseId}
        [HttpPost]
        public async Task<IActionResult> Subscribe(int courseId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Student")
            {
                return Unauthorized();
            }

            var studentId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (await _context.StudentCourses.AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId))
            {
                return BadRequest("Already subscribed.");
            }

            var subscription = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.StudentCourses.Add(subscription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // POST: /Subscription/Unsubscribe/{courseId}
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int courseId)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = int.Parse(HttpContext.Session.GetString("UserId"));

            var subscription = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (subscription == null) return NotFound();

            _context.StudentCourses.Remove(subscription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
