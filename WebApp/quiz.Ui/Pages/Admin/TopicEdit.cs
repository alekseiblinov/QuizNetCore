using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using quiz.ModelBusiness;
using quiz.Ui.Services;

namespace quiz.Ui.Pages.Admin;

/// <summary>
/// Редактор данных класса Темы.
/// </summary>
public partial class TopicEdit : IDisposable
{
// Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Ссылка на сервис для выполнения перенаправления на адреса Url. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManagerInstance { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных объектов. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private IObjectDataService<TopicModel> TopicDataService { get; set; }

    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Контекст редактирования формы.
    /// </summary>
    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    /// <summary>
    /// Id редактируемого Объекта.
    /// </summary>
    [Parameter]
    public Guid? Id { get; set; }

    private string? _returnUrlEncoded;

    /// <summary>
    /// Url страницы, на которую нужно выполнить переход (возврат) после завершения работы с редактором.
    /// </summary>
    [Parameter]
    public string? ReturnUrlEncoded
    {
        get => _returnUrlEncoded;
        set => _returnUrlEncoded = value != null ? value.Contains("/") ? HttpUtility.UrlEncode(value) : value : string.Empty;
    }

    /// <summary>
    /// Переменная содержит данные редактируемого объекта.
    /// </summary>
    private TopicModel? EditingObject { get; set; }

    /// <summary>
    /// Выбранная пользователем в списке Тема.
    /// </summary>
    private TopicModel? SelectedTopicPage { get; set; }

    /// <summary>
    /// Заголовок страницы редактора.
    /// </summary>
    private string? HeaderText { get; set; }

    /// <summary>
    /// Вносил ли пользователь изменения в данные на странице.
    /// </summary>
    private bool HasChanges { get; set; }

    /// <summary>
    /// Присутствуют ли ошибки в редактируемых данных.
    /// </summary>
    private bool HasErrors { get; set; }

    /// <summary>
    /// Текст сообщения о результате работы редактора.
    /// </summary>
    private string? ResultDescription { get; set; }

    /// <summary>
    /// Выполняется ли создание нового объекта, или редактирование существующего.
    /// </summary>
    private bool _newObjectCreating;

    /// <summary>
    /// Загружены ли данные в контрол полностью, или ещё производится загрузка.
    /// </summary>
    private bool IsLoaded { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        // Получение имени залогиненного пользователя.
        string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
        // Обнуление объекта с данными для редактирования.
        EditingObject = null;

        // Если производится редактирование записи, а не добавление новой, то
        if (Id != null)
        {
            // Получение данных элемента для редактирования через WebAPI.
            WebApiCallResult<TopicModel> webApiCallResult = await TopicDataService.GetObjectByIdAsync(Id.Value, currentUserName);

            // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
            if (webApiCallResult.HasErrors)
            {
                // Вывод пользователю информации об этом.
                HasErrors = webApiCallResult.HasErrors;
                ResultDescription = webApiCallResult.ResultDescription;
                // Дальнейшее выполнение функции прекращается.
                return;
            }

            // Если  через WebAPI получены непустые данные, то 
            if (webApiCallResult.Data != null)
            {
                // Получение данных редактируемого объекта из результата обращения к WebAPI.
                EditingObject = webApiCallResult.Data;
            }				

            // Установка текста заголовка страницы редактора.
            HeaderText = $"Изменение данных Темы \"{EditingObject?.Name}\"";
        }
        else
        {
            // Установка значения, обозначающего что выполняется создание нового объекта, а не редактирование существующего.
            _newObjectCreating = true;
            // Установка текста заголовка страницы редактора.
            HeaderText = "Добавление нового класса Визита";
            // Создание нового пустого объекта.
            EditingObject = new TopicModel(
                Guid.NewGuid(),
                string.Empty,
                0,
                DateTime.Now
            );
        }

        // Если удалось получить данные объекта для отображения, то 
        if (EditingObject != null)
        {
            // Подключение контекста редактирования с данными объекта, полученными через WebAPI.
            CurrentEditContext = new EditContext(EditingObject);
            // Подключение обработчика события изменения данных пользователем.
            CurrentEditContext.OnFieldChanged += CurrentEditContext_OnFieldChanged;
        }
        else
        {
            // Подключение пустого контекста редактирования.
            CurrentEditContext = null;
        }

        // Установка признака, обозначающего что данные в контрол загружены полностью.
        IsLoaded = true;
    }

    /// <summary>
    /// Обработка изменения выбранной пользователем в списке Темы.
    /// </summary>
    private void SelectedTopicPageChanged(TopicModel selectedElement)
    {
        // Сохранение ссылки на выбранную пользователем в списке Тему.
        SelectedTopicPage = selectedElement;
    }

    /// <summary>
    /// Обработка изменения данных пользователем на странице.
    /// </summary>
    private void CurrentEditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        // Установка признака, обозначающего что пользователь внёс изменения в данные на странице.
        HasChanges = true;
    }

    /// <summary>
    /// Обработка закрытия формы в случае успешного прохождения валидации.
    /// </summary>
    private async Task HandleValidSubmitAsync()
    {
        // Если определены данные объекта для сохранения, то 
        if (EditingObject != null)
        {
            // Получение имени залогиненного пользователя.
            string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;

            // Если производится добавление новой записи, то
            if (_newObjectCreating)
            {
                // Вызов функции добавления данных нового объекта в БД.
                WebApiCallResult<TopicModel> webApiCallResult = await TopicDataService.AddObjectAsync(EditingObject, currentUserName);

                // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                if (webApiCallResult.HasErrors)
                {
                    // Вывод пользователю информации об этом.
                    HasErrors = webApiCallResult.HasErrors;
                    ResultDescription = webApiCallResult.ResultDescription;
                    // Дальнейшее выполнение функции прекращается.
                    return;
                }

                // Сброс признака, свидетельствующего о том что происходит создание нового объекта, а не редактирование существующего - теперь он сохранен в БД и является существующим.
                _newObjectCreating = false;			
            }
            else
            {
                // Вызов функции сохранения данных объекта в БД.
                WebApiCallResult<TopicModel> webApiCallResult = await TopicDataService.UpdateObjectAsync(EditingObject, currentUserName);

                // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                if (webApiCallResult.HasErrors)
                {
                    // Вывод пользователю информации об этом.
                    HasErrors = webApiCallResult.HasErrors;
                    ResultDescription = webApiCallResult.ResultDescription;
                    // Дальнейшее выполнение функции прекращается.
                    return;
                }
            }

            // Если ошибок при сохранении не обнаружено, то
            if (!HasErrors)
            {
                // Если определена ссылка для возврата и она относится к данному веб-приложению, то
                if (!string.IsNullOrWhiteSpace(ReturnUrlEncoded) && HttpUtility.UrlDecode(ReturnUrlEncoded).StartsWith(NavigationManagerInstance.BaseUri))
                {
                    // Переход (возврат) к исходной странице.
                    NavigationManagerInstance.NavigateTo(HttpUtility.UrlDecode(ReturnUrlEncoded));
                }
            }
        }
        else
        {
            ResultDescription = "Не удалось выполнить сохранение - отсутствуют данные объекта для сохранения.";
            HasErrors = true;
        }
    }

    /// <summary>
    /// Обработка обнаружения некорректного ввода данных (непрохождения валидации).
    /// </summary>
    private void HandleInvalidSubmit()
    {
        // Формирование текста соответствующего сообщения на странице.
        ResultDescription = "Некоторые данные введены некорректно. Завершить сохранение не удалось.";
        // Установка признака, свидетельствующего о наличии ошибок.
        HasErrors = true;
    }

    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Отмена".
    /// </summary>
    private void NavigateToReturnUrl()
    {
        // Если определена ссылка для возврата и она относится к данному веб-приложению, то
        if (!string.IsNullOrWhiteSpace(ReturnUrlEncoded) && HttpUtility.UrlDecode(ReturnUrlEncoded).StartsWith(NavigationManagerInstance.BaseUri))
        {
            // Переход (возврат) к странице, указанной для возврата.
            NavigationManagerInstance.NavigateTo(HttpUtility.UrlDecode(ReturnUrlEncoded));
        }
    }

    public void Dispose()
    {
        // Если контекст формы редактирования определён, то
        if (CurrentEditContext != null)
        {
            // Отписка от событий.
            CurrentEditContext.OnFieldChanged -= CurrentEditContext_OnFieldChanged;
        }
    }
}