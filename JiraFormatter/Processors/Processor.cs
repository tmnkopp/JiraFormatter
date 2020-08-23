using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using JiraFormatter.Formatters;
namespace JiraFormatter.Processors
{
    public abstract class BaseProcessor
    {
        private List<IFormatter> formatters = new List<IFormatter>();
        public List<IFormatter> Formatters
        {
            get { return formatters; }
            set { formatters = value; }
        } 
        public virtual string Process(string content) { 
            foreach (IFormatter formatter in formatters)
            {
                content = formatter.Format(content);
            }
            return content;
        }
    }
    public class DataCallProcessor : BaseProcessor
    {
        public DataCallProcessor()
        { 
            Formatters.Add(new DescriptionCleanup());
            Formatters.Add(new QGroupTagger());
            Formatters.Add(new MetricTagger());
            Formatters.Add(new DescriptionTagger());
            Formatters.Add(new ListTagger());
        }
    }
}
