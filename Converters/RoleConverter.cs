using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Globalization;

namespace LLMChatbot.Converters;

public class RoleToColorConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
        => value is "user" ? new SolidColorBrush(Color.Parse("#6750A4"))
                           : new SolidColorBrush(Color.Parse("#4A4A4A"));

    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => throw new NotImplementedException();
}

public class RoleToAlignmentConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
        => value is "user" ? HorizontalAlignment.Right : HorizontalAlignment.Left;

    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => throw new NotImplementedException();
}