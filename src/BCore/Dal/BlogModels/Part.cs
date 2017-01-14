using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.BlogModels
{
    public class Part : Entity
    {
        public string Value { set; get; }        
        public DateTime DateTime { set; get; }

        [ForeignKey("Post")]
        public Guid PostId { set; get; }
        public virtual Post Post { set; get; }

        [ForeignKey("PartType")]
        public Guid PartTypeId { set; get; }
        public virtual PartType Type {set;get;}        
    }
}
