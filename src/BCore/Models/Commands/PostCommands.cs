using BCore.Dal.Ef;
using BCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCore.Dal.BlogModels;

namespace BCore.Models.Commands
{
    public static class PostCommands
    {
        public static async Task<Guid> SubmitCommentsAsync(PostViewModel model, Unit unit, UserManager<User> manager, ClaimsPrincipal user)
        {
            //Dal.BlogModelspost = AutoMapper.Map
        }
    }
}
