using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace quiz.Ui.Components;

/// <summary>
/// Диалог для удаления элемента.
/// </summary>
public partial class DialogDelete
{
// Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных элементов.
    /// </summary>
    [Parameter] 
    public IDeletable? ObjectDataService { get; set; }
        
    /// <summary>
    /// Ссылка на объект с данными удаляемого элемента.
    /// </summary>
    [Parameter]
    public Guid? ObjectToDeleteId { get; set; }

    /// <summary>
    /// Отображается ли данный диалог на экране пользователю.
    /// </summary>
    private bool ShowDialog { get; set; }

    /// <summary>
    /// Событие, которое возникает после успешного удаления элемента.
    /// </summary>
    [Parameter]
    public EventCallback<bool> DeletedSuccessfullyEventCallback { get; set; }

    /// <summary>
    /// Наличие ошибок при взаимодействии с WebAPI.
    /// </summary>
    private bool HasWebApiErrors { get; set; }

    /// <summary>
    /// Текст сообщения для отображения пользователю об ошибках при взаимодействии с WebAPI.
    /// </summary>
    private string? WebApiErrorsDescription { get; set; } = "Не удалось получить данные с сервера.";

    /// <summary>
    /// Отобразить диалог на экране пользователю.
    /// </summary>
    public void Show()
    {
        // Отображение диалога на экран пользователю.
        ShowDialog = true;
        // Перерисовка содержимого веб-страницы для пользователя.
        StateHasChanged();
    }

    /// <summary>
    /// Обработка нажатия пользователем кнопки "Да".
    /// </summary>
    private async void YesAsync()
    {
        WebApiCallResultRaw<bool> webApiCallResult = new WebApiCallResultRaw<bool>(false, false, 0, false);

        // Если объект для удаления известен, то
        if (ObjectToDeleteId.HasValue && ObjectDataService != null)
        {
            // Получение имени залогиненного пользователя.
            string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
            // Передача через WebAPI команды на удаление объекта в БД.
            webApiCallResult = await ObjectDataService.DeleteObjectAsync(ObjectToDeleteId.Value, currentUserName);
        }

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResult.IsSuccess)
        {
            // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            HasWebApiErrors = false;
            // Закрытие диалогового окна.
            ShowDialog = false;
            // Вызов события, обозначающего успешное удаление элемента в БД.
            await DeletedSuccessfullyEventCallback.InvokeAsync(true);
            // Перерисовка содержимого веб-страницы для пользователя.
            StateHasChanged();
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (webApiCallResult.IsUnauthorized)
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            HasWebApiErrors = true;
            // Установка текста сообщения для отображения пользователю.
            WebApiErrorsDescription = "Ошибка при обращении к WebAPI: недостаточно прав доступа.";
            // Перерисовка содержимого веб-страницы для пользователя.
            StateHasChanged();
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            HasWebApiErrors = true;
            // Установка текста сообщения для отображения пользователю.
            WebApiErrorsDescription = $"Ошибка при обращении к WebAPI. Код {webApiCallResult.StatusCode}.";
            // Перерисовка содержимого веб-страницы для пользователя.
            StateHasChanged();
        }
    }

    /// <summary>
    /// Обработка нажатия пользователем кнопки "Нет".
    /// </summary>
    private async void NoAsync()
    {
        // Закрытие диалога.
        ShowDialog = false;
        // Перерисовка содержимого веб-страницы для пользователя.
        StateHasChanged();
    }
}