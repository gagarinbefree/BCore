using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCore.Models.ViewModels.Blog;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;

namespace BCore.Models.Commands
{
    public static class UpdateCommands
    {
        /// <summary>
        /// Get post by user
        /// </summary>
        /// <param name="unit">Unit of work</param>
        /// <param name="manager">User manager</param>
        /// <param name="user">Current user</param>
        /// <returns></returns>
        public static async Task<UpdateViewModel> GetPostsByUser(Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var posts = await unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == manager.GetUserId(user) && f.Parts.Count > 0
                , 50
                , f => f.Parts, f => f.PostHashes, f => f.Comments);

            var model = Mapper.Map<UpdateViewModel>(posts);

            //var postHashes = model.Feeds.SelectMany(f => f.PostHashes).ToList();
            //postHashes.ForEach(async (f) => {
            //    f.Tag = (await PostCommands.GetHashById(f.HashId, unit)).Tag;
            //});

            //model.Feeds.ForEach(f => f.Parts = f.Parts.OrderBy(o => o.DateTime).ToList());

            //var userId = manager.GetUserId(user);
            //model.Feeds.ForEach(f => f.CanEdit = userId == f.UserId);

            return model;
        }

        /// <summary>
        /// Add new part to model
        /// </summary>
        /// <param name="model">Model</param>
        public static void AddPartToPost(UpdateViewModel model)
        {            
            model.PreviewPost.Parts.Add(Mapper.Map<PartViewModel>(model.WhatsNew));
            model.WhatsNew.Clear();
        }
    }
}
