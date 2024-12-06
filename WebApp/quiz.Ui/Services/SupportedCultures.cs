using System.Globalization;

namespace quiz.Ui.Services;

/// <summary>
/// Перечень поддерживаемых языков в UI.
/// </summary>
public static class SupportedCultures
{
    public static CultureInfo[] Cultures { get; } = new CultureInfo[]
        {
            new CultureInfo("ru-RU"),
            new CultureInfo("en-US")
        };
}