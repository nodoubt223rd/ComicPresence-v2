using System;
using System.Collections.Generic;
using System.Text;

namespace ComicPresence.Common.Text
{
    /// <summary>
    /// Utility for encoding a list of strings as a single string in a reversible way. 
    /// May be more efficient than CSV in some cases. 
    /// </summary>
    public static class StringListEncoder
    {
        private const char cPrimarySeparator = '^';
        private const char cSecondarySeparator = '`';

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string Encode(params string[] strings)
        {
            return Encode((IEnumerable<string>)strings);
        }

        /// <summary>
        /// Encodes a list of strings as a single string
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string Encode(IEnumerable<string> strings)
        {
            StringBuilder sb = new StringBuilder();
            string primarySeparatorReplacement = cPrimarySeparator.ToString() + cPrimarySeparator.ToString();
            string primarySeparatorString = cPrimarySeparator.ToString();
            string jointSeparator = cPrimarySeparator.ToString() + cSecondarySeparator.ToString();
            bool isFirst = true;
            foreach (string str in strings)
            {
                if (!isFirst)
                {
                    sb.Append(jointSeparator);
                }
                sb.Append(str.Replace(primarySeparatorString, primarySeparatorReplacement));
                isFirst = false;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Decodes an encoded string
        /// </summary>
        /// <param name="encodedStr"></param>
        /// <returns></returns>
        public static IEnumerable<string> Decode(string encodedStr)
        {
            IList<string> strings = new List<string>();

            int len = encodedStr.Length;
            StringBuilder currentStrBuilder = new StringBuilder();
            int i = 0;
            while (i < len)
            {
                if (encodedStr[i] == cPrimarySeparator)
                {
                    if (i + 1 < len && encodedStr[i + 1] == cPrimarySeparator)
                    {
                        currentStrBuilder.Append(cPrimarySeparator);
                        i += 2;
                    }
                    else if (i + 1 < len && encodedStr[i + 1] == cSecondarySeparator)
                    {
                        strings.Add(currentStrBuilder.ToString());
                        i += 2;
                        if (i >= len)
                        {
                            strings.Add(String.Empty);
                        }
                        else
                        {
                            currentStrBuilder = new StringBuilder();
                        }
                    }
                }
                else
                {
                    currentStrBuilder.Append(encodedStr[i]);
                    i++;
                    if (i >= len)
                    {
                        strings.Add(currentStrBuilder.ToString());
                    }
                }
            }

            return strings;
        }
    }
}
