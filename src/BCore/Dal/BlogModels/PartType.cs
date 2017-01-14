using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.BlogModels
{
    public class PartType : Entity
    {
        public string Name { set; get; }
        
        public virtual Part Part { set; get; }
    }
}
