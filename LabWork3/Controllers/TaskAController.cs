using LabWork3.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabWork3.Controllers
{
    public class TaskAController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("Username")) return RedirectToAction("Index", "Home");
            IList<Student> students = DataProvider.Passed;
            return View(students);
        }
    }
}
