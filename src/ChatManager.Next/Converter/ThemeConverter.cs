using System;
using System.Windows.Data;
using System.Globalization;

namespace ChatManager.Converter;

public sealed class ThemeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value is int theme)
            ? theme switch
            {
                0 => "LightTheme",
                1 => "DarkTheme",
                2 => "SystemTheme",
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            }
            : (object)"Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value is string theme)
            ? theme switch
            {
                "LightTheme" => 0,
                "DarkTheme" => 1,
                "SystemTheme" => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            }
            : (object)0;
    }
}
