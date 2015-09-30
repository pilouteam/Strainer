using System;

namespace Strainer.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparison)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return source.IndexOf(value, comparison) >= 0; 
        }

        public static bool HasValue(this string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }

        public static int ToInt(this string intStr, int fallback=default(int))
        {
            int result;
            if(int.TryParse(intStr,out result))
            {
                return result;
            }
            return fallback;
        }

    }
}

