using Demo.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Demo.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream((byte[])value);
                bitmapImage.EndInit();
                return bitmapImage;
            }
            else
            {
                return "/Demo;component/Images/default.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
