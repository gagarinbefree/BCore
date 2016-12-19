using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BCore.Dal
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { set; get; }
    }
}
