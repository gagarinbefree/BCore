using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class PostStatusLineViewModel
    {
        public UserViewModel User { set; get; }
        public DateTime PostDateTime {set; get; }
        public List<string> PostHashes { set; get; }
        public bool IsEditable { set; get; }
    }
}
