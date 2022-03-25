﻿using Microsoft.AspNetCore.Mvc;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Helpers;
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

        public async Task<IActionResult> Index(int? page, int categoryId = 0, int tagId = 0, string date = "", int authorId = 0)
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

            if (User.Identity.Name is not null)
            {
                return RedirectToAction("AdminPanel", "Admin");
            }

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
                    ViewBag.Date = date;

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

        public async Task<IActionResult> ShowPost(int id)
        {
            Post? post;

            try
            {
                post = await _postsRepository.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                var errorModel = new ErrorViewModel() { Message = e.Message };

                return View("Error", errorModel);
            }

            return View(post);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}