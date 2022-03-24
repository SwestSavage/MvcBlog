using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Extensions;
using MvcBlog.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MvcBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public AccountController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetByLoginAsync(model.Login);
                var password = model.Password;

                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                if (user is null)
                {
                    try
                    {
                        await _usersRepository.AddAsync(new User
                        {
                            Login = model.Login,
                            Password = passwordHash,
                            Name = model.Name,
                            IsAdmin = true
                        });
                    }
                    catch (Exception e)
                    {
                        var errorModel = new ErrorViewModel() { Message = e.Message };

                        return View("Error", errorModel);
                    }

                    await Authenticate(model.Login);

                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user;

                try
                {
                    user = await _usersRepository.GetByLoginAsync(model.Login);
                }
                catch (Exception e)
                {
                    var errorModel = new ErrorViewModel() { Message = e.Message };

                    return View("Error", errorModel);
                }

                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                if (user is not null && user.Password == passwordHash)
                {
                    await Authenticate(model.Login);

                    HttpContext.Session.SetObject("user", user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
