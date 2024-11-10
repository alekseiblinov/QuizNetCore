using System.Threading.Tasks;

namespace quiz.Logger;

/// <summary>
/// Методы для передачи и получения данных записей Лога с записью и чтением данных напрямую из DbContext.
/// </summary>
public interface ILogDbDirect
{
    /// <summary>
    /// Запись сообщения лога в БД.
    /// </summary>
    Task WriteLineAsync(string messageText);
}