using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LeadPro.Phone8.Converters
{
	public class ImageStreamConverter : IValueConverter
	{
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
	        var stream = value as Stream;
	        if (stream == null)
	            return null;

	        var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
	        return bitmapImage;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
	        return null;
	    }
	}
}