using System.Security.Claims;
using quiz.ModelBusiness;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace quiz.Ui.Components;

/// <summary>
/// Страница выбора темы для проведения опроса. 
/// </summary>
public partial class TopicChooseOverview : IDisposable
{
    // Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Ссылка на сервис для выполнения перенаправления на адреса Url. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManagerInstance { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных Тем. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private IObjectDataService<TopicModel> TopicsDataServiceInstance { get; set; }

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
    /// Перечень всех Тем.
    /// </summary>
    private List<TopicModel> TopicsList { get; set; } = new List<TopicModel>();

    /// <summary>
    /// Id выбранной пользователем из списка темы.
    /// </summary>
    private Guid SelectedTopicId { get; set; }

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
    /// Загружены ли данные в контрол полностью, или ещё производится загрузка.
    /// </summary>
    private bool IsLoaded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        // Получение данных текущего пользователя.
        ClaimsPrincipal currentUser = (await AuthenticationStateTask).User;

        // Если текущий пользователь не залогинен, то
        if(currentUser.Identity != null && !currentUser.Identity.IsAuthenticated)
        {
            // Формирование адреса ссылки на страницу для возвращения пользователя после успешного прохождения аутентификации.
            string returnUrl = new Uri(NavigationManagerInstance.Uri).PathAndQuery;
            returnUrl = Uri.EscapeDataString(returnUrl.TrimStart('/'));
            // Формирование ссылки для перехода к аутентификации.
            string navigateUrl = $"Identity/Account/Login?returnUrl={returnUrl}";
            // Переход на страницу аутентификации.
            NavigationManagerInstance.NavigateTo(navigateUrl, true);
        }
        else
        {
            // Получение имени залогиненного пользователя.
            string? currentUserName = currentUser.Identity?.Name;
            // Заполнение перечня Тем.
            WebApiCallResult<IEnumerable<TopicModel>> webApiCallResultQuestions = await TopicsDataServiceInstance.GetAllObjectsAsync(currentUserName);

            // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
            if (webApiCallResultQuestions.HasErrors)
            {
                // Вывод пользователю информации об этом.
                HasErrors = webApiCallResultQuestions.HasErrors;
                ResultDescription = webApiCallResultQuestions.ResultDescription;
                // Дальнейшее выполнение функции прекращается.
                return;
            }

            // Если данные через WebAPI успешно получены, то 
            if (webApiCallResultQuestions.Data != null)
            {
                TopicsList = webApiCallResultQuestions.Data.ToList();
                // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
                HasErrors = false;
            }
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        // Подключение контекста редактирования.
        CurrentEditContext = new EditContext(new TopicModel());
        // Подключение обработчика события изменения данных пользователем.
        CurrentEditContext.OnFieldChanged += CurrentEditContext_OnFieldChanged;

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
    /// Обработка нажатия на кнопку "Сохранить".
    /// </summary>
    private async void OnSubmitClick(MouseEventArgs obj)
    {
        // Если ошибок при сохранении не обнаружено, то
        if (!HasErrors)
        {
            // Формирование ссылки на страницу тестирования.
            string resultPageUrl = $"/quizoverviewpage/{SelectedTopicId}";
            // Переход (возврат) к странице тестирования.
            NavigationManagerInstance.NavigateTo(resultPageUrl);
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