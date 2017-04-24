using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.BlogModels
{
    public class Stars
    {
        public string UserId { set; get; }
        public DateTime DateTime { set; get; }        
        [ForeignKey("Post")]
        public Guid PostId { set; get; }
        public virtual Post Post { set; get; }
    }
}
