﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BCore.Models.TagHelpers
{
    [HtmlTargetElement("span", Attributes = EmailAttributeName)]
    public class GravatarImageTagHelper : TagHelper
    {
        private const string SizeAttributeName = "image-size";
        private const string EmailAttributeName = "gravatar-email";
        private const string AltTextAttributeName = "alt";
        private const string WidthAttributeName = "width";

        const string GravatarBaseUrl = "http://www.gravatar.com/avatar.php?";

        [HtmlAttributeName(EmailAttributeName)]
        public string Email { get; set; }

        [HtmlAttributeName(AltTextAttributeName)]
        public string AltText { set; get; }

        [HtmlAttributeName(SizeAttributeName)]
        public int? Size { get; set; }

        [HtmlAttributeName(WidthAttributeName)]
        public int? Width { get; set; }

        private string ToGravatarHash(string email)
        {
            var encoder = new UTF8Encoding();
            var md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(encoder.GetBytes(email.ToLower()));
            var sb = new StringBuilder(hashedBytes.Length * 2);

            for (var i = 0; i < hashedBytes.Length; i++)
                sb.Append(hashedBytes[i].ToString("X2"));

            return sb.ToString().ToLower();
        }

        private string ToGravatarUrl(string email, int? size)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email.Trim()))
                return "/images/grava.jpg";

            var sb = ToGravatarHash(email);

            var imageUrl = GravatarBaseUrl + "gravatar_id=" + sb;
            if (size.HasValue)
                imageUrl += "?s=" + size.Value;

            return imageUrl;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var str = new StringBuilder();
            var url = ToGravatarUrl(this.Email, this.Size);
            str.AppendFormat("<img src='{0}' alt='{1}' width='{2}'/>", url, AltText, this.Width);
            output.Content.AppendHtml(str.ToString());        
        }
    }
}