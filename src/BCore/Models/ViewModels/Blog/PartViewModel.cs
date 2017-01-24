using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class PartViewModel
    {
        public Guid Id { set; get; }
        public string Value { set; get; }
        public int PartType { set; get; }
        public DateTime DateTime { set; get; }

        public PartViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
