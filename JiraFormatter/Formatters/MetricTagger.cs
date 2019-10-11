using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JiraFormatter.Extentions;
namespace JiraFormatter.Formatters
{
    public class MetricTagger : IFormatter
    {  
        public string Format(string content)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            string desc = htmlDoc.DocumentNode.SelectSingleNode("//item//description").OuterHtml;
            content = content.Replace(desc , "[desc]");
            string descFormatted = desc;
            descFormatted = descFormatted.Replace("</description>", "</metric></description>");
            int cnt = 1;
            foreach (var match in Regex.Matches(descFormatted, @"<p>.{0,5}\d{1,3}\w{1,2}\..{0,5}</p>"))
            {
                string metID = match.ToString(); 
                if (cnt > 1)
                    descFormatted = descFormatted.Replace(metID, $"\n</metric>\n<metric id=\"{metID.FormatID()}\">\n{metID}");
                else
                    descFormatted = descFormatted.Replace(metID, $"\n<metric id=\"{metID.FormatID()}\">\n{metID}");
                cnt++;
                descFormatted = descFormatted.Replace(metID, $"<id-text>{metID.FormatID()}</id-text>"); 
            }

            content = content.Replace("[desc]", $"{descFormatted}"); 
            return content;
        }
    }
}
