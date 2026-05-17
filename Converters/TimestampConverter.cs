using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace LLMChatbot.Converters;

public class TimestampConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
        => value is DateTime dt ? dt.ToString("HH:mm:ss") : "";

    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => throw new NotImplementedException();
}