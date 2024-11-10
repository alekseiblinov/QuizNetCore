using eigcp.Logger.Repositories;
using eigcp.ModelBusiness;
using eigcp.ModelDb;

namespace eigcp.Api.Repositories;

/// <summary>
/// Репозиторий для управления данными записей Лога в БД.
/// *** Для ускорения работы лучше обойтись без использования Entity Framework и реализовать обращение к БД напрямую с помощью ADO.NET.
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly eigcpContext _appDbContext;

    public LogRepository(eigcpContext eigcpContext)
    {
        _appDbContext = eigcpContext;
    }

    /// <summary>
    /// Получить все записи лога из БД.
    /// </summary>
    public IEnumerable<LogRecordModel> GetAllLogRecords()
    {
        List<LogRecordModel> result = new List<LogRecordModel>();
            
        // Получение 500 последних записей.
        foreach (LogRecord? currentLogRecord in _appDbContext.LogRecords.OrderByDescending(i => i.CreatedAt).Take(500))
        {
            result.Add(new LogRecordModel(currentLogRecord.Id, currentLogRecord.Message.Substring(0, Math.Min(100, currentLogRecord.Message.Length)), currentLogRecord.CreatedAt));
        }

        return result.AsEnumerable();
    }

    /// <summary>
    /// Получение полной информации о записи лога с указанным Id.
    /// </summary>
    public LogRecordModel? GetLogRecordById(Guid logRecordId)
    {
        LogRecord? dbResult = _appDbContext.LogRecords.FirstOrDefault(i => i.Id == logRecordId);

        if (dbResult == null)
        {
            return null;
        }

        LogRecordModel result = new LogRecordModel(dbResult.Id, dbResult.Message, dbResult.CreatedAt);

        return result;
    }

    /// <summary>
    /// Запись сообщения лога в БД.
    /// </summary>
    public async Task<LogRecordModel> AddLogRecordAsync(LogRecordModel logRecord)
    {
        LogRecord editingEntity = new LogRecord()
                                  {
                                      Id = logRecord.Id,
                                      Message = logRecord.Message.Substring(0, Math.Min(1020, logRecord.Message.Length))
                                  };
        await _appDbContext.LogRecords.AddAsync(editingEntity);
        await _appDbContext.SaveChangesAsync();

        return logRecord;
    }
}