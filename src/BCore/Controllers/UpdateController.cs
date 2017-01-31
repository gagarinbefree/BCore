using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using BCore.Models.ViewModels.Blog;

namespace BCore.Controllers
{
    [Authorize]
    public class UpdateController : Controller
    {
        private Unit _unit;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            UpdateViewModel m = new UpdateViewModel();

            return View(m);
        }
    }
}
