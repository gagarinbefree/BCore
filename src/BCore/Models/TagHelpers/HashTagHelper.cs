using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models.TagHelpers
{
    [HtmlTargetElement("text", Attributes = CleanTextAttributeName)]
    public class HashTagHelper : TagHelper
    {
        private const string CleanTextAttributeName = "clean-text";

        [HtmlAttributeName(CleanTextAttributeName)]
        public string CleanText { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string outtext = HashTag.ReplaceHashTagsToLinks(CleanText);

            output.Content.AppendHtml(outtext);
        }
    }
}
