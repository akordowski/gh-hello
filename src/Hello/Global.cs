using System.Globalization;

namespace Hello;

internal static class Global
{
    public static CultureInfo CultureInfo { get; } = new("en-US");
}