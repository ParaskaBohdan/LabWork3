using LabWork3.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabWork3.Controllers
{
    public class TaskBController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("Username")) return RedirectToAction("Index", "Home");
            var _rateStudents = DataProvider.RateStudents;
            var _countLessons = DataProvider.CountLesson;
            List<(Student, double)> students = new List<(Student, double)>();
            var sorted = new SortedDictionary<int, Student>(_rateStudents).Reverse().Take(4).Select(x => (x.Value, (double)x.Key / (double)_countLessons[x.Value])).ToList();
            int i = 0;
            foreach (var temp in sorted)
            {
                if (students.Count < 4)
                {
                    (Student student, double rating) = temp;
                    rating = Math.Round(rating, 3);
                    students.Add((student, (double)rating));
                }
            }
            return View(students);
        }
    }
}
