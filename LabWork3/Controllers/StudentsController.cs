using LabWork3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace LabWork3.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("Username")) return RedirectToAction("Index", "Home");
            var _students = DataProvider.Students;
            return View(_students);
        }
    }
}
