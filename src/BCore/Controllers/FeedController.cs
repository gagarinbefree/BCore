using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Models.Commands;
using BCore.Models.ViewModels.Blog;

namespace BCore.Controllers
{
    public class FeedController : Controller
    {
        private readonly IFeedCommands _commands;

        public FeedController(IFeedCommands commands)
        {
            _commands = commands;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            FeedViewModel model = await _commands.GetLastPostsAsync(HttpContext.User);

            return View(model);
        }

        [ActionName("Search")]
        public async Task<IActionResult> SearchAsync(string tag)
        {
            FeedViewModel model = await _commands.SearchPostsByTagAsync(tag, HttpContext.User);

            if (model.RecentPosts.Count > 0)
                return View("Index", model);
            else
                return RedirectToAction("Index");
        }       
    }
}
