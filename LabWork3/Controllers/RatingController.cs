using LabWork3.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabWork3.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("Username")) return RedirectToAction("Index", "Home");
            var _ratings = DataProvider.Ratings;
            var students = DataProvider.Students;
            List<(string, Rating)> data = new List<(string, Rating)>();
            foreach (var rating in _ratings)
            {
                data.Add((students.Find(s => s.Id == rating.Id).Name, rating));
            }
            return View(data);
        }
    }
}
