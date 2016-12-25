﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BCore.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
            var post = await _unit.PostRepository.GetAsync(id, f => f.Parts);
            
            return View(Mapper.Map<PostViewModel>(post));
        }
    }
}
