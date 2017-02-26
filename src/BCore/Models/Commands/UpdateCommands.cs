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
            ICollection<Post> posts = await unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == manager.GetUserId(user) && f.Parts.Count > 0
                , 50
                , f => f.PostHashes, f => f.Comments);

            foreach(Post post in posts)
            {                
                ICollection<Part> imagePart = await unit.PartRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , false
                , f => f.PostId == post.Id && f.PartType == 1
                , 1);

                ICollection<Part> txtPart = await unit.PartRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , false
                , f => f.PostId == post.Id && f.PartType == 0
                , imagePart.Count != 0 ? 1 : 2 );

                post.Parts = txtPart.Concat(imagePart).ToList();
            }
                       
            var model = Mapper.Map<UpdateViewModel>(posts);

            model.RecentPosts.SelectMany(f => f.Hashes).ToList().ForEach(async (f) =>
            {
                Hash hash = await unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;
            });         

            var userId = manager.GetUserId(user);
            model.RecentPosts.ForEach(f =>
            {                               
                f.StatusLine = new PostStatusLineViewModel();                                       
                f.StatusLine.IsEditable = userId == f.UserId.ToString();
                f.IsPreview = true;            
            });

            return model;
        }

        /// <summary>
        /// Add new part to model
        /// </summary>
        /// <param name="model">Model</param>
        public static void AddPartToPost(UpdateViewModel model)
        {
            if (model.PreviewPost.Parts.Count() == 0)
                model.PreviewPost.DateTime = DateTime.Now;

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

        /// <summary>
        /// Delete post from DB
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="unit">Unit of work</param>
        /// <returns></returns>
        public static async Task<int> DeletePostAsync(Guid id, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var userId = await unit.PostRepository.GetValueAsync(id, f => f.UserId);

            if (userId == null)
                return -1;

            if (manager.GetUserId(user) != userId.ToString())
                return -1;

            return await unit.PostRepository.DeleteAsync(id);
        }
    }
}
