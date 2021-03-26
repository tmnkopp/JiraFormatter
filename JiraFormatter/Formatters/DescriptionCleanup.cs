using HtmlAgilityPack;
using JiraFormatter.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraFormatter.Formatters
{
    public class DescriptionCleanup : IFormatter
    { 
        public string Format(string content)
        {
            content = content.Replace("ol&gt;", "ul&gt;");
            foreach (string removeMe in new string[] { "&#160;", "&lt;br/&gt;", "&lt;b&gt;", "&lt;/b&gt;" })
                content = content.Replace($"{removeMe}", "");
            
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            string desc = htmlDoc.DocumentNode.SelectSingleNode("//item/description").InnerHtml; 
            string HTMLdesc = WebUtility.HtmlDecode(desc);

            content = content.Replace(desc, HTMLdesc);
            content = content.CleanHTML(); 

            foreach (string tag in new string[] { "p", "li", "b", "em", "strong" }) 
                content = content.Replace($"<{tag}></{tag}>", "");

            content = content.Replace($"\n\n", "\n"); 
            
            htmlDoc.LoadHtml(content);
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//item/description//p");
            Regex rex;
            foreach (var node in nodes) // REMOVE THIS STUFF
            {
            
                rex = new Regex(@"<p>.{0,5}ISSUE:.{3,100}</p>");
                if (rex.IsMatch(node.OuterHtml))
                    content = content.Replace(node.OuterHtml, "");

                rex = new Regex(@"<p>.{0,5}WHERE:.{3,100}</p>");
                if (rex.IsMatch(node.OuterHtml))
                    content = content.Replace(node.OuterHtml, "");

                rex = new Regex(@"<p>.{0,5}DETAILS.{3,100}</p>");
                if (rex.IsMatch(node.OuterHtml))
                    content = content.Replace(node.OuterHtml, "");

                rex = new Regex(@"<p>.{0,5}metrics are placeholders.{10,100}</p>");
                if (rex.IsMatch(node.OuterHtml))
                    content = content.Replace(node.OuterHtml, "");
                 
                rex = new Regex(@"<p>.{0,5}DEVELOPER.{5,500}</p>");
                if (rex.IsMatch(node.OuterHtml)) 
                    content = content.Replace(node.OuterHtml, "");

                rex = new Regex(@"<p>.{0,5}Developers,.{5,100}</p>");
                if (rex.IsMatch(node.OuterHtml))
                    content = content.Replace(node.OuterHtml, "");
            } 
            return content; 
        }
    }
}



/*
 
     
     
                 StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n');
            foreach (var line in lines)
            {
                bool includeLine = true;
                foreach (string tag in new string[] { "&lt;p&gt;&lt;b&gt;DEVELOPER&apos;S", "&lt;p&gt;&lt;em&gt;Developers," })  {
                    if (line.Contains(tag)) 
                        includeLine = false; 
                }
                if (includeLine) 
                    result.AppendFormat("{0}\n", line); 
            }
            content = result.ToString();
     
     */
