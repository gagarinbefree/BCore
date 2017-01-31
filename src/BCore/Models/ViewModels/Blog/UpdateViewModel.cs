using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class UpdateViewModel
    {
        public WhatsNewViewModel WhatsNew { set; get; }
        public List<PostViewModel> Posts { set; get; }
    }
}
