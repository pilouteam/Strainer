using System;
using System.Collections;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.UI;

namespace Strainer.MvvmCross.BindingConverter
{
	public class BoolInverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return !IsATrueValue(value, parameter, true);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            return !IsATrueValue(value, parameter, true);
		}

        protected virtual bool IsATrueValue(object value, object parameter, bool defaultValue)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool)value;
            }

            if (value is int)
            {
                if (parameter == null)
                {
                    return (int)value > 0;
                }
                else
                {
                    return (int)value > int.Parse(parameter.ToString());
                }
            }

            if (value is double)
            {
                return (double)value > 0;
            }

            if (value is string)
            {
                return !string.IsNullOrWhiteSpace(value as string);
            }

            return defaultValue;
        }
	}
}