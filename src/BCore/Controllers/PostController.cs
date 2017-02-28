using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BCore.Models.ViewModels.Blog;
using BCore.Models.Commands;
using Microsoft.AspNetCore.Authorization;

namespace BCore.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostCommands _commands;

        public PostController(IPostCommands commands)
        {
            _commands = commands;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync(Guid id)
        {
            var post = await _commands.GetPostById(id, HttpContext.User);

            return View(Mapper.Map<PostViewModel>(post));
        }

        [HttpPost]
        [ActionName("CommentSubmit")]
        public async Task<ActionResult> CommentSubmitAsync(PostViewModel m)
        {     
            if (!String.IsNullOrWhiteSpace(m.Comment.Text))      
                await _commands.SubmitCommentsAsync(m, HttpContext.User);

            return Redirect(Url.Action("Index", "Post", new { id = m.Id }) + "#commentAnchor");
        }

        [Authorize]
        [ActionName("DeleteComment")]
        public async Task<ActionResult> DeleteCommentAsync(Guid postid, Guid commentid)
        {
            await _commands.DeleteCommentAsync(commentid, HttpContext.User);
            
            return Redirect(Url.Action("Index", "Post", new { id = postid }) + "#commentInput");
        }
    }
}