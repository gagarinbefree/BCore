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
using BCore.Dal;

namespace BCore.Models.Commands
{
    public class PostCommands : IPostCommands
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IUoW _unit;

        public PostCommands(IUoW unit, IMapper map, UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = map;
            _unit = unit; 
        }

        public async Task<PostViewModel> GetPostById(Guid id, ClaimsPrincipal user)
        {
            var model = _mapper.Map<PostViewModel>(await _unit.PostRepository.GetAsync(id, f => f.Parts, f => f.PostHashes, f => f.Comments));

            model.Hashes.ForEach(async (f) =>
            {
                Hash hash = await _unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;
            });

            string userId = _userManager.GetUserId(user);

            model.StatusLine = new PostStatusLineViewModel();
            model.StatusLine.IsEditable = userId == model.UserId;
            model.IsPreview = false;

            model.Parts = model.Parts.OrderBy(f => f.DateTime).ToList();

            model.Comment = new WhatsThinkViewModel();
            model.Comments = model.Comments.OrderByDescending(f => f.DateTime).ToList();

            model.Comments.ForEach(f => f.StatusLine.IsEditable = userId == f.UserId);

            return model;
        }

        public async Task<Guid> SubmitCommentsAsync(PostViewModel model, ClaimsPrincipal user)
        {
            var comment = Mapper.Map<Comment>(model.Comment);
            comment.UserId = _userManager.GetUserId(user);
            comment.PostId = model.Id;

            return await _unit.CommentRepository.CreateAsync(comment);
        }

        public async Task<Hash> GetHashById(Guid id)
        {
            return await _unit.HashRepository.GetAsync(f => f.Id == id);
        }

        public async Task<int> DeleteCommentAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = await _unit.CommentRepository.GetValueAsync(id, f => f.UserId);

            if (userId == null)
                return -1;

            if (_userManager.GetUserId(user) != userId.ToString())
                return -1;

            return await _unit.CommentRepository.DeleteAsync(id);
        }
    }
}