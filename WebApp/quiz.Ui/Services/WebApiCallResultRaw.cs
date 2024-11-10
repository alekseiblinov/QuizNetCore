namespace quiz.Ui.Services;

/// <summary>
/// Тип содержит данные о результатах обращения к методу WebAPI.
/// </summary>
public class WebApiCallResultRaw<T>
{
    /// <summary>
    /// Данные получены через WebAPI успешно и без ошибок.
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// WebAPI сообщает о попытке неавторизованного доступа.
    /// </summary>
    public bool IsUnauthorized { get; set; }

    /// <summary>
    /// Код состояния HTTP результата обращения через WebAPI.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Объект с данными, полученный от WebAPI.
    /// </summary>
    public T? Data { get; set; }

    public WebApiCallResultRaw(bool isSuccess, bool isUnauthorized, int statusCode, T? resultData)
    {
        IsSuccess = isSuccess;
        IsUnauthorized = isUnauthorized;
        StatusCode = statusCode;
        Data = resultData;
    }
}
