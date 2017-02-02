using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels.Blog
{
    public class WhatsNewViewModel
    {

        private string text;
        public string Text
        {
            set
            {
                text = value != null ? Regex.Replace(value.Replace("\r\n", " "), "<.*?>", String.Empty) : "";
            }
            get
            {
                return text;
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            set
            {
                imageUrl = value != null ? Regex.Replace(value, "<.*?>", String.Empty) : "";
            }
            get
            {
                return imageUrl;
            }
        }

        public string VideoUrl { set; get; }
        public string Geo { set; get;   }

        public void Clear()
        {
            Text = "";
            ImageUrl = "";
            VideoUrl = "";
            Geo = "";
        }

        public string GetPartValue()
        {
            if (!String.IsNullOrWhiteSpace(Text))
                return Text;

            if (!String.IsNullOrWhiteSpace(ImageUrl))
                return ImageUrl;

            return "";
        }

        public int GetPartTypeName()
        {
            if (!String.IsNullOrWhiteSpace(Text))
                return 0;

            if (!String.IsNullOrWhiteSpace(ImageUrl))
                return 1;

            return -1;
        }
    }
}
