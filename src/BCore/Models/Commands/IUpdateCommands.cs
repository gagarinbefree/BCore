﻿using BCore.Dal.Ef;
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
        Task<UpdateViewModel> GetPostsByUserAsync(ClaimsPrincipal user, int? page = null);
        void AddPartToPost(UpdateViewModel model);
        Task<Guid> SubmitPostAsync(UpdateViewModel model, ClaimsPrincipal user);
        Task<int> DeletePostAsync(Guid id, ClaimsPrincipal user);
    }
}
