
namespace quiz.Api.Repositories;

/// <summary>
/// Интерфейс репозитория для управления данными объектов в БД.
/// </summary>
public interface IObjectRepository<T>
{
    // Получение данных всех объектов.
    IEnumerable<T> GetAllObjects();

    /// <summary>
    /// Получение данных одного объекта по его ID.
    /// </summary>
    T? GetObjectById(Guid objectId);

    /// <summary>
    /// Получение данных всех объектов в группе по ID группы.
    /// </summary>
    IEnumerable<T> GetObjectsInGroup(Guid groupId);

    /// <summary>
    /// Добавить объект в БД.
    /// </summary>
    T AddObject(T objectData);

    /// <summary>
    /// Изменить данные объекта в БД.
    /// </summary>
    T UpdateObject(T objectData);

    /// <summary>
    /// Удалить данные объекта из БД.
    /// </summary>
    void DeleteObject(Guid objectId);
}