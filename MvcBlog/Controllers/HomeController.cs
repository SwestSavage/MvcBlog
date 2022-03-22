using Microsoft.AspNetCore.Mvc;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Extensions;
using MvcBlog.Models;
using System.Diagnostics;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ITagsRepository _tagsRepository;

        public HomeController(IPostsRepository postsRepository,
            ICategoriesRepository categoriesRepository,
            ITagsRepository tagsRepository)
        {
            _postsRepository = postsRepository;
            _categoriesRepository = categoriesRepository;
            _tagsRepository = tagsRepository;
        }

        public async Task<IActionResult> Index(int categoryId = 0, int tagId = 0, string date = "", int authorId = 0)
        {
            ViewBag.Categories = await _categoriesRepository.GetAllAsync();
            ViewBag.Tags = await _tagsRepository.GetAllAsync();

            if (User.Identity.Name is not null)
            {
                return RedirectToAction("AdminPanel", "Admin");
            }

            var posts = await _postsRepository.GetAllAsync();

            if (categoryId != 0 && tagId == 0)
            {
                posts = await _postsRepository.GetAllByCategoryIdAsync(categoryId);
            }

            if (tagId != 0 && categoryId == 0)
            {
                posts = await _postsRepository.GetAllByTagIdAsync(tagId);
            }

            if (categoryId != 0 && tagId != 0)
            {
                posts = await _postsRepository.GetAllByCategoryAndTagIds(categoryId, tagId);
            }

            if (!string.IsNullOrEmpty(date))
            {
                posts = await _postsRepository.GetAllByDateAsync(Convert.ToDateTime(date));
            }

            if (authorId != 0)
            {
                posts = await _postsRepository.GetAllByAuthorIdAsync(authorId);
            }

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