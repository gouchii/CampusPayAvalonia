using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace ClientApp.Converters;

public class EmptyOrNullToDefaultConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var str = value as string;
        return string.IsNullOrWhiteSpace(str) ? "Yourself" : str;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)=>
        throw new NotImplementedException();
}