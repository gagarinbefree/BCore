using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels
{
    public class PartViewModel
    {
        public Guid Id { set; get; }

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

        public DateTime DateTime { set; get; }

        public PartViewModel()
        {
            this.DateTime = DateTime.Now;
        }
    }
}
