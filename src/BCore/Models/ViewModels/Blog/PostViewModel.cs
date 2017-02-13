﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class PostViewModel
    {
        public Guid Id { set; get; }
        public bool IsPreview { set; get; }
        public PostStatusLineViewModel StatusLine { set; get; }
        public List<PartViewModel> Parts { set; get; }
        public List<CommentViewModel> Comments { set; get; }
        public List<string> PostHashes { set; get; }                
    }
}