using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels
{
    public class PostViewModel
    {        
        public Guid Id { set; get; }

        public DateTime DateTime { set; get; }

        public string UserId { set; get; }

        public List<PartViewModel> Parts { set; get; }

        public List<string> Tags { set; get; }

        public PostViewModel()
        {
            Parts = new List<PartViewModel>();
        }        
    }
}
