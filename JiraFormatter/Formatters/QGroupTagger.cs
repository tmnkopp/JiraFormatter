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
            string id = "";
            foreach (var match in Regex.Matches(content, @"<p>.{0,2}Section.{5,250}</p>"))
            {
                string matchcontent = match.ToString();
                if (cnt <= 1) {
                    id = matchcontent.StripHTML().ReplaceWhitespace("-").FormatID().Trim(); 
                    content = content.Replace(matchcontent, $"<qgroup id=\"{id}\">{matchcontent.StripHTML()}</qgroup>");
                }    
                cnt++;
            }
            Match sectionRegex = Regex.Match(id, "Section\\d{1,2}");
            if (sectionRegex.Success)
            {
                string replacewith = sectionRegex.Value.Replace("Section", "") ;
                content = content.Replace("<qgroup", $"<sectionno>{replacewith}</sectionno>\n<qgroup");  
            }
            return content; 
        }
    }
}
