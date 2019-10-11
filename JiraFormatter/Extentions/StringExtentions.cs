using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraFormatter.Extentions
{
    public static class StringExtentions
    {
        public static string ReverseString(this string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }
        public static string ReplaceWhitespace(this string input, string ReplaceWith)
        {
            do
            {
                input = input.Replace(" ", ReplaceWith);
            } while (input.Contains(" "));
            return input;
        }
        public static string FormatID(this string input)
        {
            // Replace invalid characters with empty strings.
            input = input.StripHTML();
            try
            {
                return Regex.Replace(input, @"[^\w^\d]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            } 
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
          
        }
        public static string RemoveWhiteAndBreaks(this string input)
        {
            do
            {
                input = input.Replace(" ", "");
            } while (input.Contains(" "));
            input = input.Replace("\n", "").Replace("\r", "");
            return input;
        }
        public static string StripHTML(this string htmlString)
        {
            if (String.IsNullOrEmpty(htmlString))
                return "";
            string pattern = @"<(.|\n)*?>"; 
            return Regex.Replace(htmlString, pattern, string.Empty);
        }


        public static string CleanHTML(this string input)
        {
            string output = input; 
            do
            {
                output = output.Replace("  ", " ");
            } while (output.Contains("  "));

            output = output.Replace("> <", "><");
            string[] tags = new string[] { "p", "li", "b", "em", "strong" };
            foreach (string tag in tags)
            {
                output = output.Replace($"<{tag}></{tag}>", "");
            }

            return output;
        }
    }
}
