using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCore.Dal.BlogModels;
using BCore.Dal.Ef;
using BCore.Models.ViewModels.Blog;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace BCore.Models.Commands.Ef
{
    public class PostCommands : IPostCommands
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostCommands(UserManager<User> userManager, IMapper map)
        {
            _userManager = userManager;
            _mapper = map;
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unit">Unit of work</param>
        /// <returns></returns>
        public async Task<PostViewModel> GetPostById(Guid id, Unit unit, ClaimsPrincipal user)
        {
            var model = _mapper.Map<PostViewModel>(await unit.PostRepository.GetAsync(id, f => f.Parts, f => f.PostHashes, f => f.Comments));

            model.Hashes.ForEach(async (f) =>
            {
                Hash hash = await unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;
            });

            string userId = _userManager.GetUserId(user);

            model.StatusLine = new PostStatusLineViewModel();
            model.StatusLine.IsEditable = userId == model.UserId;
            model.IsPreview = false;

            model.Parts = model.Parts.OrderBy(f => f.DateTime).ToList();

            model.Comment = new WhatsThinkViewModel();
            model.Comments = model.Comments.OrderByDescending(f => f.DateTime).ToList();

            return model;
        }

        public async Task<Guid> SubmitCommentsAsync(PostViewModel model, Unit unit, ClaimsPrincipal user)
        {
            var comment = Mapper.Map<Comment>(model.Comment);
            comment.UserId = _userManager.GetUserId(user);
            comment.PostId = model.Id;

            return await unit.CommentRepository.CreateAsync(comment);
        }

        /// <summary>
        /// Get hash tag by id
        /// </summary>
        /// <param name="id">Hash tag id</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public async Task<Hash> GetHashById(Guid id, Unit unit)
        {
            return await unit.HashRepository.GetAsync(f => f.Id == id);
        }

        /// <summary>
        /// Delete comment from DB
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="unit">Unit of work</param>
        /// <returns></returns>
        public async Task<int> DeleteCommentAsync(Guid id, Unit unit, ClaimsPrincipal user)
        {
            var userId = await unit.CommentRepository.GetValueAsync(id, f => f.UserId);

            if (userId == null)
                return -1;

            if (_userManager.GetUserId(user) != userId.ToString())
                return -1;

            return await unit.CommentRepository.DeleteAsync(id);
        }
    }
}