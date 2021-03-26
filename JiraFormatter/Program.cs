using HtmlAgilityPack;
using JiraFormatter.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JiraFormatter.Extentions;
using JiraFormatter.Formatters;
using JiraFormatter.Processors;
using System.Reflection;

namespace JiraFormatter
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program.InvokeProcessor();  
        } 
        public static void InvokeProcessor()
        {
            DirectoryInfo dirBase = new DirectoryInfo($"{AppSettings.BasePath}");

            foreach (var dir in dirBase.GetDirectories("!*"))
            {
                string processType = dir.Name.Split('$')[1];

                string content = "";
                foreach (var file in dir.GetFiles("*.xml"))
                {
                    using (TextReader tr = File.OpenText(file.FullName))
                    {
                        content = tr.ReadToEnd();
                    }
                    DataCallProcessor processor = new DataCallProcessor();
                    content = processor.Process(content);

                    Type type = Type.GetType($"JiraFormatter.Processors.{processType}");
                    ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
                    object instance = ctor.Invoke(new object[] { });

                    MethodInfo mi = type.GetMethod("Process");
                    object result = mi.Invoke(instance, new object[] { content });


                    DirectoryInfo subDI = new DirectoryInfo($"{dir.FullName}\\_dest");
                    if (!subDI.Exists)
                        dir.CreateSubdirectory($"_dest");
                    File.WriteAllText($"{dir.FullName}\\_dest\\{file.Name}", content);

                }
            }
        }
    }

 
}
