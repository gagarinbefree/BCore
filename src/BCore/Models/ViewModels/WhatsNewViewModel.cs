using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;

namespace BCore.Models.ViewModels
{
    public class WhatsNewViewModel
    {
        public PartViewModel Part { set; get; }
        public List<PartViewModel> Parts { set; get; }
        public List<PostViewModel> Feeds { set; get; }

        public WhatsNewViewModel()
        {
            Part = new PartViewModel {
                Id = Guid.NewGuid()
            };

            Parts = new List<PartViewModel>();
        }

        public void PartToParts()
        {
            Parts.Add(new PartViewModel
            {
                ImageUrl = Part.ImageUrl
            });

            //var isMustAddPart = _isMustAddPart();

            //var part = isMustAddPart ? new PartViewModel() : Parts[Parts.Count() - 1];

            //if (!String.IsNullOrWhiteSpace(Part.Text))
            //    part.Text = Part.Text;

            //if (!String.IsNullOrWhiteSpace(Part.ImageUrl))
            //    part.ImageUrl = Part.ImageUrl;

            //if (isMustAddPart)
            //    Parts.Add(part);
        }

        private bool _isMustAddPart()
        {
            if (Parts.Count() == 0)
                return true;

            if (!String.IsNullOrWhiteSpace(Part.Text) && !String.IsNullOrWhiteSpace(Parts[Parts.Count() - 1].Text))
                return true;

            if (!String.IsNullOrWhiteSpace(Part.ImageUrl) && !String.IsNullOrWhiteSpace(Parts[Parts.Count() - 1].ImageUrl))
                return true;

            return false;
        }
    }
}
