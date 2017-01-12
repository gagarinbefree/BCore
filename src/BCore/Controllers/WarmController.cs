using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Models.ViewModels;
using BCore.Models.Commands;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;

namespace BCore.Controllers
{  
    public class WarmController : Controller
    {
        private Unit _unit;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public WarmController(BlogDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unit = new Unit(db);
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var m = await WarmCommands.GetWarmPosts(_unit, _userManager, HttpContext.User);

            return View(m);
        }
    }
}
