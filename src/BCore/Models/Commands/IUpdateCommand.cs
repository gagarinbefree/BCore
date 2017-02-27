using BCore.Dal.Ef;
using BCore.Models.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCore.Models.Commands
{
    public interface IUpdateCommands
    {
        Task<UpdateViewModel> GetPostsByUser(Unit unit, ClaimsPrincipal user);
        void AddPartToPost(UpdateViewModel model);
        Task<Guid> SubmitPostAsync(UpdateViewModel model, Unit unit, ClaimsPrincipal user);
        Task<int> DeletePostAsync(Guid id, Unit unit, ClaimsPrincipal user);
    }
}
