using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Models.ViewModels;
using System.Text.RegularExpressions;
using BCore.Dal.Ef;
using AutoMapper;
using BCore.Dal.BlogModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BCore.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private Unit _unit;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public BlogController(BlogDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unit = new Unit(db);
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            return RedirectPermanent("/Update/Index");

            /*var m = await BlogCommands.GetPostsByUser(_unit, _userManager, HttpContext.User);

            return View(m);*/
        }
        
        [HttpPost]
        public IActionResult Post(WhatsNewViewModel m)
        {
            ModelState.Clear();

            BlogCommands.AddPartToPost(m);

            return PartialView("_Post", m);
        }

        [HttpPost]
        [ActionName("Submit")]
        public async Task<ActionResult> SubmitAsync(WhatsNewViewModel m)
        {
            ModelState.Clear();

            await BlogCommands.SubmitPostAsync(m, _unit, _userManager, HttpContext.User);

            TempData["messageStatus"] = new Random(DateTime.Now.Millisecond).Next(1, 1000);

            return RedirectToAction("Index");
        }       

        [ActionName("DeletePost")]
        public async Task<ActionResult> DeletePostAsync(Guid id)
        {
            await BlogCommands.DeletePostAsync(id, _unit, _userManager, HttpContext.User);

            TempData["messageStatus"] = new Random(DateTime.Now.Millisecond).Next(1, 1000);

            return RedirectToAction("Index");
        }
    }    
}
