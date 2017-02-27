using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCore.Models.ViewModels.Blog;
using AutoMapper;
using BCore.Dal.BlogModels;
using Microsoft.AspNetCore.Identity;
using BCore.Dal.Ef;

namespace BCore.Models.Commands.Ef
{
    public class UpdateCommand : IUpdateCommands
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;        

        public UpdateCommand(UserManager<User> userManager, IMapper map)
        {
            _userManager = userManager;
            _mapper = map;
        }

        public async Task<UpdateViewModel> GetPostsByUser(Unit unit, ClaimsPrincipal user)
        {
            ICollection<Post> posts = await unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , f => f.UserId == _userManager.GetUserId(user) && f.Parts.Count > 0
                , 50
                , f => f.PostHashes, f => f.Comments);

            foreach (Post post in posts)
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
                , imagePart.Count != 0 ? 1 : 2);

                post.Parts = txtPart.Concat(imagePart).ToList();
            }

            var model = _mapper.Map<UpdateViewModel>(posts);

            model.RecentPosts.SelectMany(f => f.Hashes).ToList().ForEach(async (f) =>
            {
                Hash hash = await unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;
            });

            var userId = _userManager.GetUserId(user);
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
        public void AddPartToPost(UpdateViewModel model)
        {
            if (model.PreviewPost.Parts.Count() == 0)
                model.PreviewPost.DateTime = DateTime.Now;

            model.PreviewPost.Parts.Add(Mapper.Map<PartViewModel>(model.WhatsNew));

            model.WhatsNew.Clear();
        }

        public async Task<Guid> SubmitPostAsync(UpdateViewModel model, Unit unit, ClaimsPrincipal user)
        {
            if (model.PreviewPost.Parts.Count() == 0)
                return Guid.Empty;

            var post = Mapper.Map<Post>(model);
            post.UserId = _userManager.GetUserId(user);
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
        public async Task<int> DeletePostAsync(Guid id, Unit unit, ClaimsPrincipal user)
        {
            var userId = await unit.PostRepository.GetValueAsync(id, f => f.UserId);

            if (userId == null)
                return -1;

            if (_userManager.GetUserId(user) != userId.ToString())
                return -1;

            return await unit.PostRepository.DeleteAsync(id);
        }
    }
}