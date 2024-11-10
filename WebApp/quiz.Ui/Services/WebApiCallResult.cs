namespace quiz.Ui.Services;

/// <summary>
/// Тип содержит данные о результатах обращения к методу WebAPI, предназначенные для потребления web-страницей Razor.
/// </summary>
public class WebApiCallResult<T>
{
    /// <summary>
    /// Зарегистрированы ли ошибки при обращении через WebAPI.
    /// </summary>
    public bool HasErrors { get; set; }
    
    /// <summary>
    /// Текст для пользователя с сообщением о результате выполнения.
    /// </summary>
    public string? ResultDescription { get; set; }

    /// <summary>
    /// Объект с данными, полученный от WebAPI.
    /// </summary>
    public T? Data { get; set; }
}
