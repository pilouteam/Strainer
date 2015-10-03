using System;
using Cirrious.CrossCore.Converters;
using System.Globalization;
using UIKit;

namespace Strainer.iOS.Converters
{
    public class StringToUIImageConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            UIImage image;

            // iOS file system is case sensitive.
            // Search image with original value, then with lowercase value in case is was not found
            var found = TryGetImageFromBundle(value.ToString(), out image)
                || TryGetImageFromBundle(value.ToString().ToLowerInvariant(), out image);

            // Parameter is the fallback image, in case it was not found
            if (!found && parameter != null)
            {
                found = TryGetImageFromBundle(parameter.ToString(), out image)
                    || TryGetImageFromBundle(parameter.ToString().ToLowerInvariant(), out image);
            }

            return image;
        }

        private bool TryGetImageFromBundle(string name, out UIImage image)
        {
            image = UIImage.FromBundle (name);
            return image != null;
        }
    }
}

