using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCore.Models
{
    public static class HashTag
    {
        public static List<string> GetHashTags(string text)
        {
            List<string> res = new List<string>();

            if (String.IsNullOrWhiteSpace(text))
                return res;

            var matches = new Regex(@"(?<=#)\w+").Matches(text);
            foreach (Match m in matches)
            {
                var normalize = m.Value.ToUpper();

                if (res.FirstOrDefault(f => f.ToUpper() == normalize) == null)
                    res.Add(normalize);
            }

            return res;
        }
    }
}
