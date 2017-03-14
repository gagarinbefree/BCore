using BCore.Models.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCore.Models.Commands
{
    public interface IFeedCommands
    {
        Task<FeedViewModel> GetLastPostsAsync(ClaimsPrincipal user);
        Task<FeedViewModel> SearchPostsByTagAsync(string tag, ClaimsPrincipal user);
    }
}
