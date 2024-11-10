using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using quiz.ModelBusiness;

namespace quiz.Logger;

/// <summary>
/// Методы для передачи и получения данных записей Лога с записью и чтением данных через WebAPI.
/// </summary>
public interface ILogByHttp
{
    /// <summary>
    /// Запись сообщения лога.
    /// </summary>
    Task WriteLineAsync(string messageText, Guid securityTokenId);

    /// <summary>
    /// Получить все записи лога.
    /// </summary>
    Task<IEnumerable<LogRecordModel>> GetAllObjectsAsync(Guid securityTokenId);

    /// <summary>
    /// Получение полной информации о записи лога с указанным Id.
    /// </summary>
    Task<LogRecordModel> GetObjectDetailsAsync(Guid id, Guid securityTokenId);
}