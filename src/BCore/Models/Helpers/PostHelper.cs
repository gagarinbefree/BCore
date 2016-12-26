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
        public static async Task<int> SubmitPostAsync(WhatsNewViewModel model, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            if (model.Parts.Count > 0)
            {
                var post = Mapper.Map<Post>(model);
                post.UserId = manager.GetUserId(user);

                return await unit.PostRepository.CreateAsync(post);
            }

            return -1;
        }

        public static async Task<int> DeletePostAsync(Guid id, Unit unit)
        {
            return await unit.PostRepository.DeleteAsync(new Post { Id = id });
        }

        public static void AddPartToPost(WhatsNewViewModel model)
        {
            model.Parts.Add(Mapper.Map<PartViewModel>(model.Part));
            model.Part.Text = String.Empty;
            model.Part.ImageUrl = String.Empty;
        }

        public static async Task<ICollection<Post>> GetPostsByUser(Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            return await unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == manager.GetUserId(user) && f.Parts.Count > 0
                , 50
                , f => f.Parts);
        }
    }
}
