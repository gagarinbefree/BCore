using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BCore.Models.ViewModels;
using BCore.Models.Commands;

namespace BCore.Controllers
{
    public class PostController : Controller
    {
        private Unit _unit;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PostController(BlogDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unit = new Unit(db);
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync(Guid id)
        {
            var post = await PostCommands.GetPostById(id, _unit, HttpContext.User);

            return View(Mapper.Map<PostViewModel>(post));
        }

        [HttpPost]
        [ActionName("CommentSubmit")]
        public async Task<ActionResult> CommentSubmitAsync(PostViewModel m)
        {
            await PostCommands.SubmitCommentsAsync(m, _unit, _userManager, HttpContext.User);

            return RedirectToAction("Index", new { id = m.Id });
        }
    }
}
