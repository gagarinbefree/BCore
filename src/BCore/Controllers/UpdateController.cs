using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using BCore.Models.ViewModels.Blog;
using BCore.Models.Commands;

namespace BCore.Controllers
{
    [Authorize]
    public class UpdateController : Controller
    {
        private Unit _unit;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UpdateController(BlogDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unit = new Unit(db);
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var m = await UpdateCommands.GetPostsByUser(_unit, _userManager, HttpContext.User);

            return View(m);
        }

        [HttpPost]
        public IActionResult Post(UpdateViewModel m)
        {
            ModelState.Clear();

            UpdateCommands.AddPartToPost(m);

            return PartialView("_Post", m.PreviewPost);
        }
    }
}
