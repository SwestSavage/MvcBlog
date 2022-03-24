using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Extensions;
using MvcBlog.Helpers;
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
        public async Task<IActionResult> AdminPanel(int? page, int categoryId = 0, int tagId = 0, string date = "", int authorId = 0)
        {
            ViewBag.Categories = await _categoriesRepository.GetAllAsync();
            ViewBag.Tags = await _tagsRepository.GetAllAsync();

            IEnumerable<Post> posts;

            try
            {
                posts = await _postsRepository.GetAllAsync();

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
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            int pageSize = 3;
            int pageIndex = (page ?? 1);

            return View(PagedList<Post>.Create(posts, pageIndex, pageSize));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreatePost()
        {
            try
            {
                ViewBag.Categories = await _categoriesRepository.GetAllAsync();
                ViewBag.Tags = await _tagsRepository.GetAllAsync();
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            return View();
        }

        [Authorize]      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
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

            try
            {
                await _postsRepository.AddAsync(post);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            return RedirectToAction("AdminPanel", "Admin");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdatePost(int postId)
        {
            try
            {
                ViewBag.Categories = await _categoriesRepository.GetAllAsync();
                ViewBag.Tags = await _tagsRepository.GetAllAsync();
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            Post? post;

            try
            {
                post = await _postsRepository.GetByIdAsync(postId);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            PostViewModel model = new PostViewModel
            {
                Id = postId,
                Name = post.Name,
                ShortDescription= post.ShortDescription ?? null,
                Description= post.Description,
                UploadedFile = null,
                СategoryId = post.Сategory.Id,
                TagsIds = post.Tags.Select(t => t.Id).ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdatePost(PostViewModel model)
        {
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

            Tag? t;
            foreach (var tId in model.TagsIds)
            {
                try
                {
                    t = await _tagsRepository.GetByIdAsync(tId);
                }
                catch (Exception)
                {
                    var errorModel = new ErrorViewModel() { Message = e.Message };

                    return View("Error", errorModel);
                }

                tags.Add(t);
            }

            Post post = new Post()
            {
                Id = model.Id,
                Author = await _usersRepository.GetByLoginAsync(user.Login),
                Name = model.Name,
                ShortDescription = model.ShortDescription ?? null,
                Description = model.Description,
                ImagePath = path ?? null,
                Сategory = await _categoriesRepository.GetById(model.СategoryId),
                Tags = tags
            };

            try
            {
                await _postsRepository.UpdateAsync(post);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            return RedirectToAction("AdminPanel", "Admin");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                await _postsRepository.DeleteByIdAsync(postId);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            return RedirectToAction("AdminPanel", "Admin");
        }
    }
}
