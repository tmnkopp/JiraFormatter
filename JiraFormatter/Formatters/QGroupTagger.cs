using JiraFormatter.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraFormatter.Formatters
{
    public class QGroupTagger : IFormatter
    {
 
        public string Format(string content)
        {
            int cnt = 1;
            foreach (var match in Regex.Matches(content, @"<p>.{0,2}Section.{5,250}</p>"))
            {
                string matchcontent = match.ToString();
                if (cnt <= 1) 
                    content = content.Replace(matchcontent, $"<qgroup id=\"\">{matchcontent.StripHTML()}</qgroup>");
                cnt++;
            } 
            return content; 
        }
    }
}
