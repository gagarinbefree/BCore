using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models.TagHelpers
{
    [HtmlTargetElement("text", Attributes = HashingTextAttributeName)]
    public class HashTagHelper : TagHelper
    {
        private const string HashingTextAttributeName = "hashing-text";

        [HtmlAttributeName(HashingTextAttributeName)]
        public string HashingText { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendHtml(HashTag.ReplaceHashTagsToLinks(HashingText));
        }
    }
}
