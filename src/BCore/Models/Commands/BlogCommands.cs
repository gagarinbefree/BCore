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

namespace BCore.Models.Commands
{
    // to do DI
    public static class BlogCommands
    {
        public static async Task<Guid> SubmitPostAsync(WhatsNewViewModel model, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            if (model.Parts.Count() == 0)
                return Guid.Empty;

            var post = Mapper.Map<Post>(model);
            post.UserId = manager.GetUserId(user);
            Guid postId = await unit.PostRepository.CreateAsync(post);

            string text = model.Parts.Select(f => String.Format("{0} ", f.Text)).Aggregate((a, b) => a + b);
            List<string> tags = HashTag.GetHashTags(text);
            foreach(var tag in tags)
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

        public static async Task<WhatsNewViewModel> GetPostsByUser(Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var posts = await unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == manager.GetUserId(user) && f.Parts.Count > 0
                , 50
                , f => f.Parts, f => f.PostHashes);

            var model = Mapper.Map<WhatsNewViewModel>(posts);

            var postHashes = model.Feeds.SelectMany(f => f.PostHashes);
            foreach(var postHash in postHashes)
            {
                postHash.Tag = (await GetHashById(postHash.HashId, unit)).Tag;
            }

            return model;
        }

        public static async Task<PostViewModel> GetPostsById(Guid id, Unit unit)
        {            
            var model = Mapper.Map<PostViewModel>(await unit.PostRepository.GetAsync(id, f => f.Parts, f => f.PostHashes));
            foreach(var postHash in model.PostHashes)
            {
                postHash.Tag = (await GetHashById(postHash.HashId, unit)).Tag;
            }
            return model;
        }

        public static async Task<Hash> GetHashById(Guid id, Unit unit)
        {
            return await unit.HashRepository.GetAsync(f => f.Id == id);
        }
    }
}
