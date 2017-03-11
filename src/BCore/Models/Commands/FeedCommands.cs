using AutoMapper;
using BCore.Dal;
using BCore.Dal.Ef;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
