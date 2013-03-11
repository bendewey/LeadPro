using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace LeadPro.Win8.Common
{
    public class ImageStreamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var stream = value as Stream;

            if (stream == null)
            {
                var base64 = value as string;
                if (!string.IsNullOrEmpty(base64))
                {
                    var bytes = System.Convert.FromBase64String(base64);
                    stream = new MemoryStream(bytes);
                }
            }

            if (stream == null)
                return null;

            var bitmapImage = new BitmapImage();
            ConvertToRandomAccessStream(stream).ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        bitmapImage.SetSource(t.Result);    
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

        public static async Task<IRandomAccessStream> ConvertToRandomAccessStream(Stream source)
        {
            Stream streamToConvert = null;
            
            if (!source.CanRead)
                throw new Exception("The source cannot be read.");

            if (!source.CanSeek)
            {
                var memStream = new MemoryStream();
                await source.CopyToAsync(memStream);
                streamToConvert = memStream;
            }
            else
            {
                streamToConvert = source;
            }

            var reader = new DataReader(streamToConvert.AsInputStream());
            streamToConvert.Position = 0;
            await reader.LoadAsync((uint) streamToConvert.Length);
            var buffer = reader.ReadBuffer((uint) streamToConvert.Length);

            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await outputStream.WriteAsync(buffer);
            await outputStream.FlushAsync();

            return randomAccessStream;
        }
    }
}
