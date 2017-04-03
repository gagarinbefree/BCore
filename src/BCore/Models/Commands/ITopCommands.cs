using BCore.Models.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCore.Models.Commands
{
    interface ITopCommands
    {
        Task<TopViewModel> GetTopPostsAsync(ClaimsPrincipal user);
    }
}
