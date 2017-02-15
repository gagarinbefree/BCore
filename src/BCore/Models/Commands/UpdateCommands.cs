using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCore.Models.ViewModels.Blog;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using BCore.Dal.BlogModels;

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

            /*var postHashes = model.RecentPosts.SelectMany(f => f.PostHashes).ToList();
            postHashes.ForEach(async (f) => {
                f. = (await PostCommands.GetHashById(f.HashId, unit)).Tag;
            });*/

            model.RecentPosts.ForEach(f => f.Parts = f.Parts.OrderBy(o => o.DateTime).ToList());

            var userId = manager.GetUserId(user);

            model.RecentPosts.ForEach(f => 
            {
                f.StatusLine = new PostStatusLineViewModel();
                //f.StatusLine.IsEditable = userId == f.StatusLine.User.UserId.ToString();
            });

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

        public static async Task<Guid> SubmitPostAsync(UpdateViewModel model, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            if (model.PreviewPost.Parts.Count() == 0)
                return Guid.Empty;

            var post = Mapper.Map<Post>(model);
            post.UserId = manager.GetUserId(user);
            Guid postId = await unit.PostRepository.CreateAsync(post);

            string text = model
                .PreviewPost
                .Parts
                .Where(f => f.PartType == 0)
                .Select(f => String.Format("{0} ", f.Value)).Aggregate((a, b) => a + b);
            if (String.IsNullOrWhiteSpace(text))
                return postId;

            List<string> tags = HashTag.GetHashTags(text);
            foreach (var tag in tags)
            {
                var existTag = await unit.HashRepository.GetAsync(f => f.Tag == tag);
                Guid hashId = existTag == null
                    ? await unit.HashRepository.CreateAsync(new Hash { Tag = tag }) : existTag.Id;

                PostHash postHash = new PostHash
                {
                    PostId = postId,
                    HashId = hashId
                };

                await unit.PostHashRepository.CreateAsync(postHash);
            }

            return postId;
        }
    }
}
