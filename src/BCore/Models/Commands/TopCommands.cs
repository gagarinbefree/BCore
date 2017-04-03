using AutoMapper;
using BCore.Dal;
using BCore.Dal.BlogModels;
using BCore.Dal.Ef;
using BCore.Models.ViewModels.Blog;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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

        
        public async Task<TopViewModel> GetTopPostsAsync(ClaimsPrincipal user)
        {
            ICollection<Post> posts = await _unit.PostRepository.GetAllAsync(
                f => f.Comments.Count()
                , true
                , null
                , 50
                , f => f.PostHashes, f => f.Comments);

            return await _createViewModel(posts, user);
        }

        private async Task<TopViewModel> _createViewModel(ICollection<Post> posts, ClaimsPrincipal user)
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

            var model = _mapper.Map<TopViewModel>(posts);

            model.RecentPosts.SelectMany(f => f.Hashes).ToList().ForEach(async (f) =>
            {
                Hash hash = await _unit.HashRepository.GetAsync(f.Id);
                f.Tag = hash.Tag;               
            });

            int ii = 1;
            var userId = _userManager.GetUserId(user);
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
