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
        private readonly IPostsRepository _postsRepository;

        public HomeController(ILogger<HomeController> logger,
            IUsersRepository usersRepository,
            IPostsRepository postsRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _postsRepository = postsRepository;
        }

        public async Task<IActionResult> Index()
        {
            User user = HttpContext.Session.GetObject<User>("user");

            if (User.Identity.Name is not null)
            {
                return RedirectToAction("AdminPanel", "Admin");
            }

            var posts = await _postsRepository.GetAllAsync();

            return View(posts);
        }

        public async Task<IActionResult> ShowPost(int id)
        {
            var post = await _postsRepository.GetByIdAsync(id);

            return View(post);
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