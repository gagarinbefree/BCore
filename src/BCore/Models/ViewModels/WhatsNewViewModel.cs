using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;

namespace BCore.Models.ViewModels
{
    public class WhatsNewViewModel
    {
        public PreviewPartViewModel PreviewPart { set; get; }
        public List<PartViewModel> Parts { set; get; }
        public List<PostViewModel> Feeds { set; get; }

        public WhatsNewViewModel()
        {
            PreviewPart = new PreviewPartViewModel();

            Parts = new List<PartViewModel>();
        }
    }
}
