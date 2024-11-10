using System.Web;
using quiz.ModelBusiness;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace quiz.Ui.Areas.Identity.Pages.Account;

/// <summary>
/// Данные о роли и вхождении пользователя в роль. 
/// </summary>
class UserIsMemberOfRole
{
    /// <summary>
    /// Данные о роли.
    /// </summary>
    public RoleModel Role { get; set; }
    /// <summary>
    /// Входит ли пользователь в роль.
    /// </summary>
    public bool IsMember { get; set; }

    public UserIsMemberOfRole(RoleModel role, bool isMember)
    {
        Role = role;
        IsMember = isMember;
    }
}

/// <summary>
/// Редактор данных Пользователя.
/// </summary>
public partial class UserEdit : IDisposable
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
    private IObjectDataService<UserModel> UserDataService { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных объектов Типов проектов. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private IObjectDataService<RoleModel> RolesDataServiceInstance { get; set; }

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
        set => _returnUrlEncoded =
            value != null
                ? value.Contains("/") ? HttpUtility.UrlEncode(value) : value
                : string.Empty;
    }

    /// <summary>
    /// Перечень ролей с отметками вхождения в них пользователя.
    /// </summary>
    private List<UserIsMemberOfRole> UserRoles { get; set; } = new List<UserIsMemberOfRole>();

    /// <summary>
    /// Переменная содержит данные редактируемого объекта.
    /// </summary>
    private UserModel? EditingObject { get; set; }

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
    /// Счётчик чекбоксов ролей.
    /// </summary>
    private int checkBoxesCounter;

    /// <summary>
    /// Имя (логин) пользователя до изменения при редактировании.
    /// </summary>
    private string? _loginSaved;

    /// <summary>
    /// Загружены ли данные в контрол полностью, или ещё производится загрузка.
    /// </summary>
    private bool IsLoaded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Получение имени залогиненного пользователя.
        string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
        // Заполнение перечня ролей.
        WebApiCallResult<IEnumerable<RoleModel>> webApiCallResultRoles = await RolesDataServiceInstance.GetAllObjectsAsync(currentUserName);

        // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
        if (webApiCallResultRoles.HasErrors)
        {
            // Вывод пользователю информации об этом.
            HasErrors = webApiCallResultRoles.HasErrors;
            ResultDescription = webApiCallResultRoles.ResultDescription;
            // Дальнейшее выполнение функции прекращается.
            return;
        }

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResultRoles.Data != null)
        {
            // Заполнение списка всех существующих ролей.
            foreach (RoleModel currentRole in webApiCallResultRoles.Data)
            {
                UserRoles.Add(new UserIsMemberOfRole(currentRole, false));
            }

            // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            HasErrors = false;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        // Если производится редактирование записи, а не добавление новой, то
        if (Id != null)
        {
            // Получение имени залогиненного пользователя.
            string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
            // Обнуление объекта с данными для редактирования.
            EditingObject = null;
            // Получение информации о Пользователе.
            WebApiCallResult<UserModel> webApiCallResult = await UserDataService.GetObjectByIdAsync(Id.Value, currentUserName);

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

            // Если удалось получить данные редактируемого объекта из БД.
            if (EditingObject != null)
            {
                // Трюк. При редактировании пользователя значение пароля заменяется пробелами. Такой пароль пройдёт валидацию модели. но на уровне контроллера не будет сохранён в БД (пароль пользователя останется прежним), так как там установлена проверка if (!string.IsNullOrWhiteSpace(objectModel.Password)).
                EditingObject.Password = "                ";
                // Сохранение значения имени (логина) пользователя до редактирования.
                _loginSaved = EditingObject.Login;
            }

            // Установка текста заголовка страницы редактора.
            HeaderText = $"Изменение данных пользователя \"{EditingObject?.Login}\"";
        }
        else
        {
            // Установка значения, обозначающего что выполняется создание нового объекта, а не редактирование существующего.
            _newObjectCreating = true;
            // Установка текста заголовка страницы редактора.
            HeaderText = "Добавление нового Пользователя";
            // Создание нового пустого объекта.
            EditingObject = new UserModel(
                Guid.NewGuid(),
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now,
                new List<RoleModel>()
            );
        }

        // Если удалось получить данные объекта для отображения, то 
        if (EditingObject != null)
        {
            foreach (var currentRole in UserRoles)
            {
                currentRole.IsMember = EditingObject != null && EditingObject.Roles.Any(i => i.Id == currentRole.Role.Id);
            }

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
            // Очистка данных о ролях пользователя.
            EditingObject.Roles.Clear();

            // Заполнение списка ролей пользователя, в которые он входит.
            foreach (var currentUserRole in UserRoles.Where(i => i.IsMember))
            {
                EditingObject.Roles.Add(currentUserRole.Role);
            }

            // Если производится добавление новой записи, то
            if (_newObjectCreating)
            {
                // Получение имени залогиненного пользователя.
                string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
                // Получение данных элементов списка через WebAPI.
                WebApiCallResult<IEnumerable<UserModel>> webApiCallResultUsersList = await UserDataService.GetAllObjectsAsync(currentUserName);

                // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                if (webApiCallResultUsersList.HasErrors)
                {
                    // Вывод пользователю информации об этом.
                    HasErrors = webApiCallResultUsersList.HasErrors;
                    ResultDescription = webApiCallResultUsersList.ResultDescription;
                    // Дальнейшее выполнение функции прекращается.
                    return;
                }

                // Если имя пользователя не занято в БД, то
                if (webApiCallResultUsersList.Data != null && webApiCallResultUsersList.Data.All(i => i.Login != EditingObject.Login))
                {
                    // Вызов функции добавления данных нового объекта в БД.
                    WebApiCallResult<UserModel> webApiCallResult = await UserDataService.AddObjectAsync(EditingObject, currentUserName);

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
                    ResultDescription = "Такое имя пользователя уже зарегистрировано в БД.";
                    HasErrors = true;
                }
            }
            else
            {
                // Получение имени залогиненного пользователя.
                string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
                // Получение данных элементов списка через WebAPI.
                WebApiCallResult<IEnumerable<UserModel>> webApiCallResultUsersList = await UserDataService.GetAllObjectsAsync(currentUserName);

                // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                if (webApiCallResultUsersList.HasErrors)
                {
                    // Вывод пользователю информации об этом.
                    HasErrors = webApiCallResultUsersList.HasErrors;
                    ResultDescription = webApiCallResultUsersList.ResultDescription;
                    // Дальнейшее выполнение функции прекращается.
                    return;
                }

                // Если имя пользователя было изменено и новое имя пользователя занято в БД, то
                if (EditingObject.Login != _loginSaved
                    && webApiCallResultUsersList.Data != null
                    && webApiCallResultUsersList.Data.Any(i => i.Login == EditingObject.Login))
                {
                    ResultDescription = "Такое имя пользователя уже зарегистрировано в БД.";
                    HasErrors = true;
                }
                else
                {
                    // Вызов функции сохранения данных объекта в БД.
                    WebApiCallResult<UserModel> webApiCallResult = await UserDataService.UpdateObjectAsync(EditingObject, currentUserName);

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
        if (CurrentEditContext != null)
        {
            // Отписка от событий.
            CurrentEditContext.OnFieldChanged -= CurrentEditContext_OnFieldChanged;
        }
    }
}