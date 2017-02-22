using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class PostViewModel
    {
        public Guid Id { set; get; }            
        public string UserId { set; get; }         
        public bool IsPreview { set; get; }        
        public DateTime DateTime { set; get; }
        public PostStatusLineViewModel StatusLine { set; get; }
        public List<PartViewModel> Parts { set; get; }
        public List<CommentViewModel> Comments { set; get; }
        public List<HashViewModel> Hashes { set; get; }               
                       
        public PostViewModel()
        {            
            Parts = new List<PartViewModel>();
        }
    }
}
