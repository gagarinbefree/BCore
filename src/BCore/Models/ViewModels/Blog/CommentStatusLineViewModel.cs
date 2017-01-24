using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class CommentStatusLineViewModel
    {
        public UserViewModel User { set; get; }
        public string Text { set; get; }
        public bool IsEditable { set; get; }
    }
}
