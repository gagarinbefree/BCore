using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class FeedViewModel
    {
        public List<PostViewModel> RecentPosts { set; get; }
    }
}
