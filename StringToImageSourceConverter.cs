using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LitroMetr
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                try
                {
                    // Создаем абсолютный URI для изображения
                    Uri imageUri = new Uri($"pack://application:,,,{path}", UriKind.Absolute);
                    return new ImageSourceConverter().ConvertFrom(imageUri);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}