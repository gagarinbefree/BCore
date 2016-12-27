﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models
{
    public static class HashTag
    {
        private static string _pattern = @"(?<=#)\w+";

        public static List<string> GetHashTags(string text)
        {
            List<string> res = new List<string>();

            if (String.IsNullOrWhiteSpace(text))
                return res;

            var matches = new Regex(_pattern).Matches(text);
            foreach (Match m in matches)
            {
                var normalize = m.Value.ToUpper();

                if (res.FirstOrDefault(f => f.ToUpper() == normalize) == null)
                    res.Add(normalize);
            }

            return res;
        }

        public static string ReplaceHashTagsToLinks(string clearText)
        {
            string res = Regex.Replace(clearText, _pattern, new MatchEvaluator(_link));

            return res;
        }

        private static string _link(Match m)
        {           
            return String.Format("<a href=\"#\">{0}</a>", m);
        }
    }
}
