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
            var posts = await _unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == _userManager.GetUserId(HttpContext.User) && f.Parts.Count > 0
                , 50
                , f => f.Parts);
            
            return View(Mapper.Map<WhatsNewViewModel>(posts));
        }
        
        [HttpPost]
        public IActionResult Post(WhatsNewViewModel m)
        {
            ModelState.Clear();

            m.Parts.Add(Mapper.Map<PartViewModel>(m.Part));
            m.Part.Text = String.Empty;
            m.Part.ImageUrl = String.Empty;

            return PartialView("_Post", m);
        }

        [HttpPost]
        [ActionName("Submit")]
        public async Task<ActionResult> SubmitAsync(WhatsNewViewModel m)
        {
            ModelState.Clear();

            if (m.Parts.Count > 0)
            {
                var post = Mapper.Map<Post>(m);
                post.UserId = _userManager.GetUserId(HttpContext.User);

                await _unit.PostRepository.CreateAsync(post);
            }

            TempData["messageStatus"] = new Random(DateTime.Now.Millisecond).Next(1, 1000);

            return RedirectToAction("Index");
        }       

        [ActionName("DeletePost")]
        public async Task<ActionResult> DeletePostAsync(Guid id)
        {
            await _unit.PostRepository.DeleteAsync(new Dal.BlogModels.Post { Id = id });

            TempData["messageStatus"] = new Random(DateTime.Now.Millisecond).Next(1, 1000);

            return RedirectToAction("Index");
        }
    }    
}
