using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Extensions;
using MvcBlog.Models;

namespace MvcBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminController(IPostsRepository postsRepository,
            ICategoriesRepository categoriesRepository,
            ITagsRepository tagsRepository,
            IWebHostEnvironment appEnvironment, IUsersRepository usersRepository)
        {
            _postsRepository = postsRepository;
            _categoriesRepository = categoriesRepository;
            _tagsRepository = tagsRepository;
            _appEnvironment = appEnvironment;
            _usersRepository = usersRepository;
        }

        [Authorize]
        public async Task<IActionResult> AdminPanel()
        {
            ViewData["LoggedIn"] = User.Identity.Name;

            var posts = await _postsRepository.GetAllAsync();

            return View(posts);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreatePost()
        {
            ViewData["LoggedIn"] = User.Identity.Name;

            ViewBag.Categories = await _categoriesRepository.GetAllAsync();
            ViewBag.Tags = await _tagsRepository.GetAllAsync();

            return View();
        }

        [Authorize]      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            ViewData["LoggedIn"] = User.Identity.Name;
            User user = HttpContext.Session.GetObject<User>("user");

            string? path = null;

            if (model.UploadedFile is not null)
            {
                path = "/Files/" + model.UploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.UploadedFile.CopyToAsync(fileStream);
                }               
            }


            List<Tag> tags = new();

            foreach (var tId in model.TagsIds)
            {
                var t = await _tagsRepository.GetByIdAsync(tId);
                tags.Add(t);
            }

            Post post = new Post()
            {
                Author = await _usersRepository.GetByLoginAsync(user.Login),
                Name = model.Name,
                ShortDescription = model.ShortDescription ?? null,
                Description = model.Description,
                ImagePath = path ?? null,
                Сategory = await _categoriesRepository.GetById(model.СategoryId),
                Tags = tags
            };

            await _postsRepository.AddAsync(post);

            return RedirectToAction("AdminPanel", "Admin");
        }
    }
}
