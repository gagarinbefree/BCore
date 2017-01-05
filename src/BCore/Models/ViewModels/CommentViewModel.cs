using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { set; get; }

        public Guid UserId { set; get; }

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
    }
}
