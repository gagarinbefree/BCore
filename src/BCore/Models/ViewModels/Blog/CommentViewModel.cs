using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class CommentViewModel
    {
        public CommentStatusLineViewModel StatusLine { set; get; }
        public string Text { set; get; }
    }
}
