using BCore.Dal.Ef;
using BCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCore.Dal.BlogModels;
using AutoMapper;

namespace BCore.Models.Commands
{
    public static class PostCommands
    {
        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unit">Unit of work</param>
        /// <returns></returns>
        public static async Task<PostViewModel> GetPostById(Guid id, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var model = Mapper.Map<PostViewModel>(await unit.PostRepository.GetAsync(id, f => f.Parts, f => f.PostHashes, f => f.Comments));
            
            model.PostHashes.ForEach(async (f) => {
                f.Tag = (await PostCommands.GetHashById(f.HashId, unit)).Tag;
            });

            if (user.Identity.IsAuthenticated)
                model.Comment = new CommentViewModel();

            model.Comments = model.Comments.OrderByDescending(f => f.DateTime).ToList();
            var userId = manager.GetUserId(user);
            model.Comments.ForEach(f => f.CanEdit = userId == f.UserId);

            model.Parts = model.Parts.OrderBy(f => f.DateTime).ToList();

            return model;
        }

        public static async Task<Guid> SubmitCommentsAsync(PostViewModel model, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var comment = Mapper.Map<Comment>(model.Comment);
            comment.UserId = manager.GetUserId(user);
            comment.PostId = model.Id;

            return await unit.CommentRepository.CreateAsync(comment);
        }

        /// <summary>
        /// Get hash tag by id
        /// </summary>
        /// <param name="id">Hash tag id</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static async Task<Hash> GetHashById(Guid id, Unit unit)
        {
            return await unit.HashRepository.GetAsync(f => f.Id == id);
        }

        /// <summary>
        /// Delete comment from DB
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="unit">Unit of work</param>
        /// <returns></returns>
        public static async Task<int> DeleteCommentAsync(Guid id, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            var userId = await unit.CommentRepository.GetValueAsync(id, f => f.UserId);

            if (userId == null)
                return -1;

            if (manager.GetUserId(user) != userId.ToString())
                return -1;

            return await unit.CommentRepository.DeleteAsync(id);
        }
    }
}
