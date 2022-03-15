using Microsoft.AspNetCore.Mvc;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Extensions;
using MvcBlog.Models;
using System.Diagnostics;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsersRepository _usersRepository;

        public HomeController(ILogger<HomeController> logger,
            IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }

        public IActionResult Index()
        {
            ViewData["LoggedIn"] = Convert.ToString(User.Identity.Name);
            User user = HttpContext.Session.GetObject<User>("user");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}