using BCore.Dal.BlogModels;
using BCore.Dal.Ef;
using BCore.Models.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCore.Models.Commands
{
    public interface IPostCommands
    {
        Task<PostViewModel> GetPostById(Guid id,ClaimsPrincipal user);        
        Task<Hash> GetHashById(Guid id);
        Task<int> DeleteCommentAsync(Guid id, ClaimsPrincipal user);
        Task<Guid> SubmitCommentsAsync(PostViewModel model, ClaimsPrincipal user);
    }
}
