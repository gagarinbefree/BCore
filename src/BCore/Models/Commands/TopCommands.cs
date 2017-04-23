using AutoMapper;
using BCore.Dal;
using BCore.Dal.BlogModels;
using BCore.Dal.Ef;
using BCore.Models.ViewModels.Blog;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCore.Models.Commands
{
    public class TopCommands : ITopCommands
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IUoW _unit;

        public TopCommands(IUoW unit, IMapper map, UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = map;
            _unit = unit;
        }

        
        public async Task<TopViewModel> GetTopPostsAsync(ClaimsPrincipal user, int? page = null)
        {
            ICollection<Post> posts = await _unit.PostRepository.GetAllAsync(
                null,
                page == null ? 0 : (page - 1) * PagerViewModel.ItemsOnPage,
                PagerViewModel.ItemsOnPage,
                f => f.PostHashes, f => f.Comments);

            TopViewModel model = await _createViewModel(posts.OrderByDescending(f => f.Comments.Count()).ThenByDescending(f => f.DateTime).ToList(), user, page);

            model.Pager = new PagerViewModel(await _unit.PostRepository.CountAsync(), page == null ? 1 : (int)page);

            return model;
        }

        private async Task<TopViewModel> _createViewModel(ICollection<Post> posts, ClaimsPrincipal user, int? page)
        {            
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

            var model = _mapper.Map<TopViewModel>(posts);

            model.RecentPosts.SelectMany(f => f.Hashes).ToList().ForEach(async (f) =>
            {
                Hash hash = await _unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;               
            });
            
            var userId = _userManager.GetUserId(user);
            int ii = page != null ? (int)page * PagerViewModel.ItemsOnPage - 1 : 1;
            model.RecentPosts.ForEach(f =>
            {
                f.StatusLine = new PostStatusLineViewModel();
                f.StatusLine.IsEditable = userId == f.UserId.ToString();
                f.IsPreview = true;
                f.TopNumber = ii;

                ii++;
            });

            return model;
        }
    }
}
