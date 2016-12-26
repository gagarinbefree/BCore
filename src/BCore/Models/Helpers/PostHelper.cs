using AutoMapper;
using BCore.Dal.BlogModels;
using BCore.Dal.Ef;
using BCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;

namespace BCore.Models.Helpers
{
    // to do DI
    public static class BlogHelper
    {
        public static async Task<int> SubmitPostAsync(WhatsNewViewModel model, Unit _unit, UserManager<User> _userManager, ClaimsPrincipal user)
        {
            if (model.Parts.Count > 0)
            {
                var post = Mapper.Map<Post>(model);
                post.UserId = _userManager.GetUserId(user);

                return await _unit.PostRepository.CreateAsync(post);
            }

            return -1;
        }

        public static async Task<int> DeletePostAsync(Guid id, Unit _unit)
        {
            return await _unit.PostRepository.DeleteAsync(new Post { Id = id });
        }

        public static void AddPartToPost(WhatsNewViewModel model)
        {
            model.Parts.Add(Mapper.Map<PartViewModel>(model.Part));
            model.Part.Text = String.Empty;
            model.Part.ImageUrl = String.Empty;
        }

        public static async Task<ICollection<Post>> GetPostsByUser(Unit _unit, UserManager<User> _userManager, ClaimsPrincipal user)
        {
            return await _unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == _userManager.GetUserId(user) && f.Parts.Count > 0
                , 50
                , f => f.Parts);
        }
    }
}
