using LabWork3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LabWork3.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            var name = HttpContext.Session.GetString("Username");
            Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {name} left");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}
