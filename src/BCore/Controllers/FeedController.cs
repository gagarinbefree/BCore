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
            FeedViewModel m = await _commands.GetLastPostsAsync(HttpContext.User);

            return View(m);
        }

        [ActionName("Search")]
        public async Task<IActionResult> SearchAsync(string tag)
        {
            FeedViewModel m = await _commands.SearchPostsByTagAsync(tag, HttpContext.User);

            return View("Index", m);
        }
    }
}
