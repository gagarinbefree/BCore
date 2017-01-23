using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.BlogModels
{
    public class Hash : Entity
    {
        public string Tag { set; get; }
        public string DateTime { set; get; }
        public virtual ICollection<PostHash> PostHashes { set; get; }

        public Hash()
        {
            PostHashes = new List<PostHash>();
        }
    }
}
