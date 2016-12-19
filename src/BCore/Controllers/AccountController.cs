using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Models.ViewModels;
using System.Text.RegularExpressions;
using BCore.Dal.Ef;
using AutoMapper;
using BCore.Dal.BlogModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Signin")]
        public async Task<IActionResult> SigninAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserName };
                // добавляем пользователя
                var resultCreateUser = await _userManager.CreateAsync(user, model.Password);
                if (resultCreateUser.Succeeded)
                {
                    var resultAddToRole = await _userManager.AddToRoleAsync(user, "User");
                    if (!resultAddToRole.Succeeded)
                    {
                        foreach(var error in resultAddToRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }                        

                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultCreateUser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect username or password");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("LogOff")]
        public async Task<IActionResult> LogOffAsync()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}