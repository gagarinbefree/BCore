using BCore.Dal.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.BlogModels
{
    public class Post : Entity
    {
        public string UserId { set; get; }
        public DateTime DateTime { set; get; }
        
        //public string PostId { set; get; } // repost

        public virtual ICollection<Part> Parts { set; get; }
        public virtual ICollection<Comment> Comments { set; get; }                
        public virtual ICollection<PostHash> PostHashes { set; get; }
        
        //public virtual ICollection<Stars> Stars { set; get; }

        public Post()
        {
            Parts = new List<Part>();
            Comments = new List<Comment>();
            PostHashes = new List<PostHash>();
        }
    }        
}
