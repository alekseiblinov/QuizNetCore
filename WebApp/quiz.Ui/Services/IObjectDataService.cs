namespace quiz.Ui.Services;

/// <summary>
/// Интерфейс сервиса для передачи в БД и получения из нее данных объектов указанного типа.
/// </summary>
public interface IObjectDataService<T> : IDeletable
{
    /// <summary>
    /// Получить все объекты указанного типа.
    /// </summary>
    Task<WebApiCallResult<IEnumerable<T>>> GetAllObjectsAsync(string? userName);
    
    /// <summary>
    /// Получить из БД данные объекта с указанным ID.
    /// </summary>
    Task<WebApiCallResult<T>> GetObjectByIdAsync(Guid id, string? userName);

    /// <summary>
    /// Получить из БД данные объектов, относящихся к определенной группе с указанным ID. Например, список всех Контролов LabelLine класса Страницы визита с указанным ID.
    /// </summary>
    Task<WebApiCallResult<IEnumerable<T>>> GetObjectsInGroupAsync(Guid id, string? userName);
    
    /// <summary>
    /// Добавить новый объект в БД.
    /// </summary>
    Task<WebApiCallResult<T>> AddObjectAsync(T element, string? userName);

    /// <summary>
    /// Изменить (редактировать) данные объекта в БД.
    /// </summary>
    Task<WebApiCallResult<T>> UpdateObjectAsync(T element, string? userName);
}