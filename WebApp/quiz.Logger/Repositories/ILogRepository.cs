using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using quiz.ModelBusiness;

namespace quiz.Logger.Repositories;

/// <summary>
/// Интерфейс репозитория для управления данными записей Лога.
/// </summary>
public interface ILogRepository
{
    /// <summary>
    /// Получить все записи лога из БД.
    /// </summary>
    IEnumerable<LogRecordModel> GetAllLogRecords();

    /// <summary>
    /// Получение полной информации о записи лога с указанным Id.
    /// </summary>
    LogRecordModel GetLogRecordById(Guid logRecordId);

    /// <summary>
    /// Запись сообщения лога в БД.
    /// </summary>
    Task<LogRecordModel> AddLogRecordAsync(LogRecordModel logRecord);
}