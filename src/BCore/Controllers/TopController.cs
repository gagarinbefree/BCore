using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Models.ViewModels.Blog;
using BCore.Models.Commands;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BCore.Controllers
{
    public class TopController : Controller
    {
        private readonly IFeedCommands _commands;

        public TopController(IFeedCommands commands)
        {
            _commands = commands;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            FeedViewModel model = await _commands.GetTopPostsAsync(HttpContext.User);

            return View("Index", model);
        }
    }
}
