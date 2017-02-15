using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class UserViewModel
    {
        public Guid UserId { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
    }
}
