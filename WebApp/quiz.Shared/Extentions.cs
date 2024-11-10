namespace quiz.Shared;

/// <summary>
/// Различные расширяющие методы общего назначения.
/// </summary>
public static class Extentions
{
    /// <summary>
    /// Удаление суффикса из строки.
    /// По материалам https://stackoverflow.com/questions/5284591/how-to-remove-a-suffix-from-end-of-string.
    /// </summary>
    public static string RemoveSuffix(this string sourceString, string suffixToRemove)
    {
        return sourceString.EndsWith(suffixToRemove) 
            ? sourceString.Substring(0, sourceString.Length - suffixToRemove.Length) 
            : sourceString;
    }
}