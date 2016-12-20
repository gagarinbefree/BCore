using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BCore.Dal.Ef;

namespace BCore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [ActionName("Profile")]
        public async Task<IActionResult> ProfileAsync(string returnUrl = null)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            return View(new ProfileViewModel { ReturnUrl = returnUrl, Email = user.Email });
        }

        [HttpPost]
        [ActionName("Profile")]
        public async Task<IActionResult> ProfileAsync(ProfileViewModel m)
        {            
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var resultSetEmail = await _userManager.SetEmailAsync(user, m.Email);
                if (!resultSetEmail.Succeeded)
                {
                    foreach (var error in resultSetEmail.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            return View(m);
        }
    }
}
