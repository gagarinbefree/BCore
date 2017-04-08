﻿using AutoMapper;
using BCore.Dal;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCore.Models.ViewModels.Blog;
using BCore.Dal.BlogModels;
using System.Security.Claims;
using PagedList.Core;

namespace BCore.Models.Commands
{
    public class FeedCommands : IFeedCommands
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IUoW _unit;

        public FeedCommands(IUoW unit, IMapper map, UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = map;
            _unit = unit;
        }

        public async Task<FeedViewModel> GetLastPostsAsync(ClaimsPrincipal user, int? page = default(int?))
        {
            ICollection<Post> posts = await _unit.PostRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , true
                , page == null ? 0 : page * 10
                , 10
                , f => f.PostHashes, f => f.Comments);
            
            return await _createViewModel(posts, user, page);
        }

        public async Task<FeedViewModel> SearchPostsByTagAsync(string tag, ClaimsPrincipal user, int? page = default(int?))
        {
            Hash hash = await _unit.HashRepository.GetAsync(f => f.Tag == tag);
            ICollection<PostHash> tagPostHashes = await _unit.PostHashRepository.GetAllAsync(f => f.HashId == hash.Id
            , 50
            , f => f.Post, f => f.Hash);
            // hack. in tagPostHashes Posts collection contains one PostHases item. Why?
            tagPostHashes.ToList().ForEach(async (f) =>
            {
                f.Post = await _unit.PostRepository.GetAsync(f.PostId, i => i.PostHashes, i => i.Comments);
            });
            ICollection<Post> posts = tagPostHashes.Select(f => f.Post).OrderByDescending(f => f.DateTime).ToList();

            return await _createViewModel(posts, user, page);
        }
                
        private async Task<FeedViewModel> _createViewModel(ICollection<Post> posts, ClaimsPrincipal user, int? page)
        {
            foreach (Post post in posts)
            {
                ICollection<Part> imagePart = await _unit.PartRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , false
                , f => f.PostId == post.Id && f.PartType == 1
                , 1);

                ICollection<Part> txtPart = await _unit.PartRepository.GetAllAsync<DateTime>(
                f => f.DateTime
                , false
                , f => f.PostId == post.Id && f.PartType == 0
                , imagePart.Count != 0 ? 1 : 2);

                post.Parts = txtPart.Concat(imagePart).ToList();
            }

            var model = _mapper.Map<FeedViewModel>(posts);            
            model.RecentPosts.SelectMany(f => f.Hashes).ToList().ForEach(async (f) =>
            {
                Hash hash = await _unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;
            });

            var userId = _userManager.GetUserId(user);
            model.RecentPosts.ForEach(f =>
            {
                f.StatusLine = new PostStatusLineViewModel();
                f.StatusLine.IsEditable = userId == f.UserId.ToString();
                f.IsPreview = true;
            });

            model.Page = page;

            return model;
        }       
    }
}
