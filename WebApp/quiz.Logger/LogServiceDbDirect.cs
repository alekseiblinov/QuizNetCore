using System;
using System.Threading.Tasks;
using quiz.Logger.Repositories;
using quiz.ModelBusiness;

namespace quiz.Logger;

/// <summary>
/// Сервис для управления сообщениями лога.
/// </summary>
public class LogServiceDbDirect : ILogDbDirect
{
    /// <summary>
    /// Объект репозитория для управления данными Лога в БД.
    /// </summary>
    private readonly ILogRepository _logRepository;

    public LogServiceDbDirect(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    /// <summary>
    /// Запись сообщения лога в БД.
    /// </summary>
    public async Task WriteLineAsync(string text)
    {
        LogRecordModel newRecord = new LogRecordModel(Guid.NewGuid(), text, null);
        await _logRepository.AddLogRecordAsync(newRecord);
    }
}