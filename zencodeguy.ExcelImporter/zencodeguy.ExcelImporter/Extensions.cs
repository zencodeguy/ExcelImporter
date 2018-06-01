using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace zencodeguy.ExcelImporter
{
    public static class Extensions
    {
        public static IEnumerable<string> ReadLine(this string s)
        {
            string line;
            using (var sr = new StringReader(s))
            {
                while((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if(line.Length == 0)
                    {
                        continue;
                    }
                    if (!line.StartsWith("#"))
                    {
                        while (!line.EndsWith(";"))
                        {
                            line = line.Trim();
                            var nextLine = sr.ReadLine().Trim();
                            while (nextLine.StartsWith("\t"))
                            {
                                nextLine = nextLine.Substring(1);
                            }
                            if (line.Length > 0)
                            {
                                line += " " + nextLine;
                            }
                        }
                    }

                    if (line.EndsWith(";"))
                    {
                        yield return line.Substring(0, line.Length - 1);
                    }
                    else
                    {
                        yield return line;
                    }
                }
            }
        }

        public static string GetNextWord(this string s, int index)
        {
            int count = 0;

            if(s[index] == ' ')
            {
                index++;
            }

            while(index + count < s.Length && s[index + count] != ' ')
            {
                count++;
            }

            return s.Substring(index, count).Trim();
        }

        public static List<string> ToList(this string s, char delimiter)
        {
            return new List<string>(s.Split(delimiter));
        }

        public static Tuple<string, int> ExtractDelimitedSection(this string s, char open, char close, int index)
        {
            while(s[index] != open)
            {
                index++;
                if(index >= s.Length)
                {
                    throw new InvalidOperationException("String does not contain opening delimited.");
                }
            }

            index++;

            int count = 0;
            while(index + count < s.Length)
            {
                if(s[index + count] == close)
                {
                    if(s[index + count - 1] == '\\')
                    {
                        count++;
                    }
                    else
                    {
                        var r = s.Substring(index, count);
                        r = r.Replace("\\", "").Trim();
                        return new Tuple<string, int>(r, count);
                    }
                }

                count++;
            }

            throw new InvalidOperationException("The string contains no closing delimiter.");
        }

        public static List<string> ExtractListItems(this string s, char open, char close, char delimiter, int index)
        {
            List<string> items = new List<string>();
            int count = 0;

            while(index + count < s.Length)
            {
                var result = s.ExtractDelimitedSection('"', '"', index + count);
                items.Add(result.Item1);
                count += result.Item2 + 2;
                while(index + count < s.Length && s[index + count] != open)
                {
                    count++;
                }
            }

            return items;
        }

        public static string AsString(this List<string> l)
        {
            if(l.Count == 0)
            {
                return l[0];
            }

            StringBuilder sb = new StringBuilder();
            bool firstString = true;

            foreach(var s in l)
            {
                if(firstString)
                {
                    firstString = false;
                }
                else
                {
                    sb.Append(",");
                }
                sb.Append("\"" + s + "\"");
            }
            return sb.ToString();
        }
    }
}
