using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks; 
namespace JiraFormatter.Formatters
{
    public class DescriptionTagger : IFormatter
    {  
        public string Format(string content)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//metric/p");
            if (nodes == null)
                return content;
            foreach (var node in nodes)
            {
                Regex rex = new Regex(@"<p>.{15,500}</p>");
                if (rex.IsMatch(node.OuterHtml))
                {
                    content = content.Replace(node.OuterHtml, $"<question-text>{node.InnerHtml}</question-text>"); 
                }
            } 
            return content; 
        }
    }
}
