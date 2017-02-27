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
        Task<PostViewModel> GetPostById(Guid id, Unit unit, ClaimsPrincipal user);        
        Task<Hash> GetHashById(Guid id, Unit unit);
        Task<int> DeleteCommentAsync(Guid id, Unit unit, ClaimsPrincipal user);
        Task<Guid> SubmitCommentsAsync(PostViewModel model, Unit unit, ClaimsPrincipal user);
    }
}
