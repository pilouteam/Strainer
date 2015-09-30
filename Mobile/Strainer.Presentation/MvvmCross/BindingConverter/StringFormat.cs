using System;
using Cirrious.CrossCore.Converters;
using System.Globalization;

namespace Strainer.BindingConverter
{
    public class StringFormat : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return value;

            var format = HasPlaceholder(parameter as string)
                ? parameter.ToString()
                : "{0:" + parameter.ToString()  + "}";

            if (value is TimeSpan)
            {
                value = DateTime.MinValue+(TimeSpan)value;
            }
            return string.Format(culture, format, value);
        }

        private bool HasPlaceholder(string candidate)
        {
            // Detects if a string has placeholders (ie: "Hello, {0}")
            if (candidate == null)
            {
                return false;
            }
            if (candidate.Length < 3)
            {
                return false;
            }
            if (candidate.IndexOf('{') < 0)
            {
                return false;
            }

            // Search for "{" but not "{{" (escaped brace)
            // Caveat: Will not detect "{{{0}}}";
            for (int i = 1; i < candidate.Length - 1; i++)
            {
                if (candidate[i] == '{' && candidate[i + 1] != '{' && candidate[i - 1] != '{')
                {
                    return true;
                }
            }
            return false;

        }

    }
}

