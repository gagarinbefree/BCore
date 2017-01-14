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

        private string value;
        public string Value
        {
            set
            {
                if (this.PartType.Name == "text")
                    Value = value != null ? Regex.Replace(value.Replace("\r\n", " "), "<.*?>", String.Empty) : "";

                if (this.PartType.Name == "image")
                    Value = value != null ? Regex.Replace(value, "<.*?>", String.Empty) : "";
            }
            get
            {
                return value;
            }
        }

        public PartTypeViewModel PartType { set; get; }

        public DateTime DateTime { set; get; }

        public PartViewModel()
        {
            this.DateTime = DateTime.Now;
        }
    }
}
