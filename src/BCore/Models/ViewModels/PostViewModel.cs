using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { set; get; }

        public DateTime DateTime { set; get; }

        public string UserId { set; get; }

        public List<PartViewModel> Parts { set; get; }

        public List<PostHasheViewModel> PostHashes { set; get; } 
        
        public List<CommentViewModel> Comments { set; get; }

        public CommentViewModel Comment { set; get; }

        public bool CanEdit { set; get; }

        public PostViewModel()
        {
            Parts = new List<PartViewModel>();
            PostHashes = new List<PostHasheViewModel>();
        }        
    }
}
