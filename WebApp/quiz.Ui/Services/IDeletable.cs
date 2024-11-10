namespace quiz.Ui.Services;

/// <summary>
/// Методы для реализации удаления данных записей в сервисе взаимодействия с данными.
/// </summary>
public interface IDeletable
{
    /// <summary>
    /// Удалить из БД элемент с указанным ID.
    /// </summary>
    public Task<WebApiCallResultRaw<bool>> DeleteObjectAsync(Guid objectId, string? userName);
}
