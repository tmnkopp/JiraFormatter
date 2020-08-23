using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraFormatter.Formatters
{
    public class ListTagger : IFormatter
    {
        public string Format(string content)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//metric//ul");
            if (nodes == null)
                return content;
            foreach (var node in nodes)
            {
                bool IsPickList = true;
                HtmlNodeCollection listitemnodes = node.SelectNodes("li");
                int count = listitemnodes.Count();
                foreach (var listitemnode in listitemnodes)
                { 
                    if (IsPickList)
                        IsPickList = !listitemnode.InnerHtml.Contains("Yes/No");
                    if (IsPickList)
                        IsPickList = !listitemnode.InnerHtml.Contains("Checkbox");
                    if (IsPickList)
                        IsPickList = !listitemnode.InnerHtml.Contains("Numeric");
                    if (IsPickList)
                        IsPickList = !listitemnode.InnerHtml.Contains("N/A"); 
                }
                 

                if (IsPickList)
                    IsPickList = !(count <= 1); 

                if (IsPickList) {
                    content = content.Replace(node.OuterHtml, $"<picklist>{node.InnerHtml}</picklist>");
                } else {
                    content = content.Replace(node.OuterHtml, $"<control>{node.InnerHtml}</control>");
                }
            }
            return content;
        }
    }
}
