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
using BCore.Dal;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace BCore.Models.Commands
{
    public class UpdateCommands : IUpdateCommands
    {
        private IUoW _unit;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;        

        public UpdateCommands(IUoW unit, IMapper map, UserManager<User> userManager)
        {
            _unit = unit;            
            _mapper = map;
            _userManager = userManager;
        }

        public async Task<UpdateViewModel> GetPostsByUserAsync(ClaimsPrincipal user, int? page)
        {
            ICollection<Post> posts = await _unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime,
                SortOrder.Descending,
                f => f.UserId == _userManager.GetUserId(user) && f.Parts.Count > 0,
                page == null ? 0 : (page - 1) * PagerViewModel.ItemsOnPage,
                PagerViewModel.ItemsOnPage,
                f => f.PostHashes, f => f.Comments);

            foreach (Post post in posts)
            {
                ICollection<Part> imagePart = await _unit.PartRepository.GetAllAsync<DateTime>(
                f => f.DateTime,
                SortOrder.Ascending,
                f => f.PostId == post.Id && f.PartType == 1,
                null,
                1);

                ICollection<Part> txtPart = await _unit.PartRepository.GetAllAsync<DateTime>(
                f => f.DateTime,
                SortOrder.Ascending,
                f => f.PostId == post.Id && f.PartType == 0,
                null,
                imagePart.Count != 0 ? 1 : 2);

                post.Parts = txtPart.Concat(imagePart).ToList();
            }

            var model = _mapper.Map<UpdateViewModel>(posts);

            model.RecentPosts.SelectMany(f => f.Hashes).ToList().ForEach(async (f) =>
            {
                Hash hash = await _unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;
            });

            string userId = _userManager.GetUserId(user);
            model.RecentPosts.ForEach(f =>
            {
                f.StatusLine = new PostStatusLineViewModel();
                f.StatusLine.IsEditable = userId == f.UserId.ToString();
                f.IsPreview = true;
            });
            
            model.Pager = new PagerViewModel(await _unit.PostRepository.CountAsync(), page == null ? 1 : (int)page);

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

        public async Task<Guid> SubmitPostAsync(UpdateViewModel model, ClaimsPrincipal user)
        {
            if (model.PreviewPost.Parts.Count() == 0)
                return Guid.Empty;

            var post = Mapper.Map<Post>(model);
            post.UserId = _userManager.GetUserId(user);
            Guid postId = await _unit.PostRepository.CreateAsync(post);
            
            IEnumerable<PartViewModel> textParts = model
                .PreviewPost
                .Parts
                .Where(f => f.PartType == 0);

            if (textParts.Count() == 0)
                return postId;

            string text = textParts.Select(f => String.Format("{0} ", f.Value)).Aggregate((a, b) => a + b);
            if (String.IsNullOrWhiteSpace(text))
                return postId;

            List<string> tags = HashTag.GetHashTags(text);
            foreach (var tag in tags)
            {
                var existTag = await _unit.HashRepository.GetAsync(f => f.Tag == tag);
                Guid hashId = existTag == null
                    ? await _unit.HashRepository.CreateAsync(new Hash { Tag = tag }) : existTag.Id;

                PostHash postHash = new PostHash
                {
                    PostId = postId,
                    HashId = hashId
                };

                await _unit.PostHashRepository.CreateAsync(postHash);
            }

            return postId;
        }

        /// <summary>
        /// Delete post from DB
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="unit">Unit of work</param>
        /// <returns></returns>
        public async Task<int> DeletePostAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = await _unit.PostRepository.GetValueAsync(id, f => f.UserId);

            if (userId == null)
                return -1;

            if (_userManager.GetUserId(user) != userId.ToString())
                return -1;

            return await _unit.PostRepository.DeleteAsync(id);
        }
    }
}