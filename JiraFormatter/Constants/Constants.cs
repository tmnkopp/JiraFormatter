using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraFormatter.Constants
{
    public static class AppSettings
    {
         public static string BasePath = ConfigurationManager.AppSettings["BasePath"].ToString();
         public static string CommandChar = ConfigurationManager.AppSettings["CommandChar"].ToString();

       }
}
