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
using Microsoft.AspNetCore.Authorization;

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
            if (!String.IsNullOrWhiteSpace(m.Comment.Text))      
                await PostCommands.SubmitCommentsAsync(m, _unit, _userManager, HttpContext.User);

            return Redirect(Url.Action("Index", "Post", new { id = m.Id }) + "#commentInput");
        }

        [Authorize]
        [ActionName("DeleteComment")]
        public async Task<ActionResult> DeleteCommentAsync(Guid postid, Guid commentid)
        {
            await PostCommands.DeleteCommentAsync(commentid, _unit, _userManager, HttpContext.User);

            TempData["messageStatus"] = new Random(DateTime.Now.Millisecond).Next(1, 1000);

            return Redirect(Url.Action("Index", "Post", new { id = postid }) + "#commentInput");
        }
    }
}