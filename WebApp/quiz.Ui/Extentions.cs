namespace quiz.Ui;

/// <summary>
/// Различные расширяющие методы общего назначения.
/// </summary>
public static class Extentions
{
    /// <summary>
    /// Неизбежный объект класса Random для последующего использования.
    /// </summary>
    private static readonly Random _randomGenerator = new Random();  

    /// <summary>
    /// Получение элементов из списка, являющихся соседними к указанному. Используется для позиционирования на следующую запись после удаления строки в списке.
    /// По материалам https://stackoverflow.com/questions/8759849/get-previous-and-next-item-in-a-ienumerable-using-linq
    /// </summary>
    public static IEnumerable<T> FindSandwichedItem<T>(this IEnumerable<T> items, Predicate<T> matchFilling)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        if (matchFilling == null)
            throw new ArgumentNullException(nameof(matchFilling));

        return FindSandwichedItemImpl(items, matchFilling);
    }

    private static IEnumerable<T> FindSandwichedItemImpl<T>(IEnumerable<T> items, Predicate<T> matchFilling)
    {
        using (IEnumerator<T> iter = items.GetEnumerator())
        {
            T previous = default(T);
            while (iter.MoveNext())
            {
                if (matchFilling(iter.Current))
                {
                    yield return previous;
                    yield return iter.Current;
                    if (iter.MoveNext())
                        yield return iter.Current;
                    else
                        yield return default(T);
                    yield break;
                }
                previous = iter.Current;
            }
        }

        // If we get here nothing has been found so return three default values
        yield return default(T); // Previous
        yield return default(T); // Current
        yield return default(T); // Next
    }

    /// <summary>
    /// Перемешивание случайным образом элементов переданного списка.
    /// По материалам https://stackoverflow.com/questions/273313/randomize-a-listt.
    /// </summary>
    /// <typeparam name="T">Тип элементов в коллекции</typeparam>
    /// <param name="list">Коллекция, элементы которой следует перемешать</param>
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  

        while (n > 1) {  
            n--;  
            int k = _randomGenerator.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}