using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace FrmBlog.Helpers
{
    public static class StringExtensions
    {
        public static string SlashWithTire(this string text)
        {
            if(!string.IsNullOrEmpty(text))
                text = text.Replace("/", "-");
            return text;
        }
     
        public static string RemoveTirnak(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                str = str.Replace("\"", "");          
            return str;
        }
        public static string ConvertWebUrl(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = TrimWithTire(text);
                text = SlashWithTire(text);
                text = ConvertTrToEn(text);
                text = RemoveTirnak(text);
                text = text.Replace("?", "");
                text = text.Replace("@@", "");
                text = text.Replace("%t%", "");
                text = text.Replace(":", "");
                text = text.Replace(".", "");
                text = text.Replace(@"\", "-");
                text = text.Replace("%", "");
                text = text.Replace("&", "");
                text = text.Replace("+", "");
            }
            return text;
        }
        public static string GetAvatarBySize(this string txt,int oldSize,int newSize)
        {
        //http://www.gravatar.com/avatar/04b84967d828551db995e592be3fab8b?s=32&d=identicon&r=PG
            string ns ="s="+newSize.ToString();
            string os = "s=" + oldSize.ToString();
            return txt.Replace(os,ns);
        }
        public static string Join(this string[] values, string joinText)
        {
            StringBuilder result = new StringBuilder();
            if (values.Length == 0) return string.Empty;
            result.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                result.Append(joinText);
                result.Append(values[i]);
            }
            return result.ToString();
        }
        public static bool IsNumeric(this string text)
        {
            double Result;
            return double.TryParse(text, out Result);  // TryParse routines were added in Framework version 2.0.
        }
        public static string ConvertUtf8ToTr(this string val)
        {
            val = val.Replace("Ã§", "ç");
            val = val.Replace("Ã", "Ç");
            val = val.Replace("Ä", "ğ");
            val = val.Replace("Ä", "Ğ");
            val = val.Replace("Ä±", "ı");
            val = val.Replace("Ä°", "İ");
            val = val.Replace("Ã¶", "ö");
            val = val.Replace("Ã", "Ö");
            val = val.Replace("Å", "ş");
            val = val.Replace("Å", "Ş");
            val = val.Replace("Ã¼", "ü");
            val = val.Replace("Ã", "Ü");
            return val;
        }
        public static string ConvertTrToEn(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace('ç', 'c');
                text = text.Replace('Ç', 'C');
                text = text.Replace('ı', 'i');
                text = text.Replace('İ', 'I');
                text = text.Replace('ğ', 'g');
                text = text.Replace('Ğ', 'G');

                text = text.Replace('ş', 's');
                text = text.Replace('Ş', 'S');
                text = text.Replace('ö', 'o');
                text = text.Replace('Ö', 'O');

                text = text.Replace('ü', 'u');
                text = text.Replace('Ü', 'U');
            }
            return text;
        }
        public static string TrimWithTire(this string text)
        {
            if(!string.IsNullOrEmpty(text))
                text = text.Replace(" ", "-");
            return text;
        }
        public static string TrimWithElipsis(this string text, int length)
        {
            if (text.Length <= length) return text;
            return text.Substring(0, length) + "...";
        }

        /// <summary>
        /// replacement for String.Format
        /// </summary>
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// prettily renders property names
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Pretty(this string text)
        {
            return DeCamel(text).Replace("_", " ");
        }

        public static void PrettyTest()
        {
            Console.WriteLine("hello_worldIAmYourNemesis".Pretty());
        }

        /// <summary>
        /// turns HelloWorld into Hello World
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DeCamel(this string text)
        {
            return Regex.Replace(text, @"([A-Z])", @" $&").Trim();
        }

        public static void DeCamelTest()
        {
            Console.WriteLine("HelloWorldIAmYourNemesis".DeCamel());
        }
    }
}