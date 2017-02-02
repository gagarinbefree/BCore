using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.BlogModels
{
    public class PostHash : Entity
    {
        public Guid PostId { set; get; }
        public Post Post { set; get; }
        public Guid HashId { set; get; }
        public Hash Hash { set; get; }             
    }
}
