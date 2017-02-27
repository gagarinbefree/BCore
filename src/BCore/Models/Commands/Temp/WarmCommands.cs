using AutoMapper;
using BCore.Dal.Ef;
using BCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCore.Models.Commands
{
    public static class WarmCommands
    {
        public static async Task<WarmViewModel> GetWarmPosts(Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var posts = await unit.PostRepository.GetAllAsync<DateTime>(f => f.DateTime
                , true
                , f => f.Parts.Count > 0
                , 50
                , f => f.Parts, f => f.PostHashes, f => f.Comments);

            var model = Mapper.Map<WarmViewModel>(posts);

            var postHashes = model.Feeds.SelectMany(f => f.PostHashes).ToList();
            postHashes.ForEach(async (f) => {
                f.Tag = (await PostCommands.GetHashById(f.HashId, unit)).Tag;
            });

            model.Feeds.ForEach(f => f.Parts = f.Parts.OrderBy(o => o.DateTime).ToList());

            var userId = manager.GetUserId(user);
            model.Feeds.ForEach(f => f.CanEdit = userId == f.UserId);

            return model;
        }
    }
}
