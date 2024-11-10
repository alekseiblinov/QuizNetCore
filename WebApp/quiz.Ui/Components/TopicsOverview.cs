using System.Web;
using quiz.Logger;
using quiz.ModelBusiness;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace quiz.Ui.Components;

/// <summary>
/// Контрол для просмотра списка Тем и кнопок управления для них.
/// </summary>
public partial class TopicsOverview
{
// Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Ссылка на сервис для выполнения перенаправления на адреса Url. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManagerInstance { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных элементов. Внедряется с помощью DI.
    /// </summary>
    [Inject] 
    private IObjectDataService<TopicModel> ObjectDataService { get; set; }

    /// <summary>
    /// Ссылка на сервис для взаимодействия с данными лога. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private ILogDbDirect LogService { get; set; }
    
    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Список отображаемых в таблице элементов.
    /// </summary>
    private IEnumerable<TopicModel>? Objects { get; set; }

    /// <summary>
    /// Диалог для удаления элемента.
    /// </summary>
    private DialogDelete ObjectDeleteDialog { get; set; } = null!;

    /// <summary>
    /// Данные выбранного в таблице элемента.
    /// </summary>
    public TopicModel? SelectedElement;
        
    /// <summary>
    /// Данные элемента таблицы, который будет выбран после завершения удаления элемента.
    /// </summary>
    private TopicModel? SelectedElementAfterDelete { get; set; }

    /// <summary>
    /// Загружены ли данные в контрол полностью, или ещё производится загрузка.
    /// </summary>
    private bool IsLoaded { get; set; }

    /// <summary>
    /// Наличие ошибок при взаимодействии с WebAPI.
    /// </summary>
    private bool HasWebApiErrors { get; set; }

    /// <summary>
    /// Текст сообщения для отображения пользователю об ошибках при взаимодействии с WebAPI.
    /// </summary>
    private string? WebApiErrorsDescription { get; set; } = "Не удалось получить данные с сервера.";

    /// <summary>
    /// Недоступность кнопки "Вверх".
    /// </summary>
    private bool IsUpButtonDisabled
    {
        get
        {
            bool result = true;

            // Если пользователь выбрал элемент в списке, то
            if (SelectedElement != null && Objects != null)
            {
                // Получение OrderNumber выбранного в списке элемента.
                int orderNumberOfSelectedElement = SelectedElement.OrderNumber;

                if (orderNumberOfSelectedElement > 1)
                {
                    // Поиск элемента с OrderNumber меньше, чем у выбранного.
                    TopicModel? previousElementInList = Objects
                        .Where(i => i.OrderNumber < orderNumberOfSelectedElement)
                        .OrderBy(i => i.OrderNumber)
                        .LastOrDefault();

                    // Если удалось найти элемент с OrderNumber меньше, чем у выбранного, то
                    if (previousElementInList != null)
                    {
                        // Выбранный пользователем в списке элемент может быть передвинут вверх.
                        result = false;
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Недоступность кнопки "Вниз".
    /// </summary>
    private bool IsDownButtonDisabled 
    {
        get
        {
            bool result = true;

            // Если пользователь выбрал элемент в списке, то
            if (SelectedElement != null && Objects != null)
            {
                // Получение OrderNumber выбранного в списке элемента.
                int orderNumberOfSelectedElement = SelectedElement.OrderNumber;

                // Поиск элемента с OrderNumber больше, чем у выбранного.
                TopicModel? nextElementInList = Objects
                    .Where(i => i.OrderNumber > orderNumberOfSelectedElement)
                    .OrderBy(i => i.OrderNumber)
                    .FirstOrDefault();

                // Если удалось найти элемент с OrderNumber больше, чем у выбранного, то
                if (nextElementInList != null)
                {
                    // Выбранный пользователем в списке элемент может быть передвинут вниз.
                    result = false;
                }
            }

            return result;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        // Вызов функции обновления из БД списка элементов, отображаемого на форме.
        await PopulateElementsAsync();
        // Установка признака, обозначающего что данные в контрол загружены полностью.
        IsLoaded = true;
    }

    /// <summary>
    /// Обновление из БД данных списка, отображаемого на форме.
    /// </summary>
    private async Task PopulateElementsAsync()
    {
        // Получение имени залогиненного пользователя.
        string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
        // Получение из БД списка элементов.
        WebApiCallResult<IEnumerable<TopicModel>> webApiCallResult = await ObjectDataService.GetAllObjectsAsync(currentUserName);

        // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
        if (webApiCallResult.HasErrors)
        {
            // Вывод пользователю информации об этом.
            HasWebApiErrors = webApiCallResult.HasErrors;
            WebApiErrorsDescription = webApiCallResult.ResultDescription;
            // Дальнейшее выполнение функции прекращается.
            return;
        }

        // Если удалось получить из БД список элементов, то
        if (webApiCallResult.Data != null)
        {
            // Список объектов для отображения в списке предварительно сортируется.
            Objects = webApiCallResult.Data.OrderBy(i => i.OrderNumber);
        }
    }

    /// <summary>
    /// Обработка выбора пользователем элемента из списка.
    /// </summary>
    private async void SelectRowAsync(Guid recordId)
    {
        // Формирование данных выбранного из списка пользователем элемента. 
        SelectedElement = Objects?.FirstOrDefault(i => i.Id == recordId);
    }

    /// <summary>
    /// Определение названия css-класса строки таблицы.
    /// </summary>
    private string GetRowHtmlClass(Guid objectId)
    {
        string result = string.Empty;

        // Если Id элемента строки совпадает с выбранным пользователем элементом таблицы, то
        if (SelectedElement != null && SelectedElement.Id == objectId)
        {
            // Строка становится выделенной в таблице.
            result = "bg-info border-info rounded";
        }

        return result;
    }

    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Добавить".
    /// </summary>
    private void TopicAdd()
    {
        // Переход к странице редактирования Тем.
        NavigationManagerInstance.NavigateTo($"topicedit/{HttpUtility.UrlEncode(NavigationManagerInstance.Uri)}");
    }           
        
    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Редактировать".
    /// </summary>
    private void TopicEdit(Guid topicId)
    {
        // Переход к странице редактирования Тем.
        string targetUrl = $"topicedit/{topicId}/{HttpUtility.UrlEncode(NavigationManagerInstance.Uri)}";
        NavigationManagerInstance.NavigateTo(targetUrl);
    }       

    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Удалить".
    /// </summary>
    private async void ObjectDeleteAsync()
    {
        // Если пользователь выбрал элемент в списке, то 
        if (SelectedElement != null && Objects != null)
        {
            // Получение элементов из списка, являющихся соседними к удаляемому.
            List<TopicModel> sandwichedItems = Objects.FindSandwichedItem(item => item.Id == SelectedElement.Id).ToList();
            TopicModel previousItem = sandwichedItems[0];
            //var deletedItem = sandwichedItems[1];
            TopicModel nextItem = sandwichedItems[2];

            // Если в списке есть следующий элемент, то 
            if (nextItem != null)
            {
                // Предыдущий элемент из списка станет активным после завершения удаления выбранного пользователем элемента из таблицы.
                SelectedElementAfterDelete = nextItem;
            }
            // Если в списке есть предыдущий элемент, то 
            else if (previousItem != null)
            {
                // Следующий элемент из списка активным после завершения удаления выбранного пользователем элемента из таблицы.
                SelectedElementAfterDelete = previousItem;
            }
            else
            {
                // Никакой элемент не станет активным после завершения удаления пользователем элемента из таблицы.
                SelectedElementAfterDelete = null;
            }

            // Отображение popup-диалога подтверждения удаления элемента из таблицы.
            ObjectDeleteDialog.Show();
        }
    }        
        
    /// <summary>
    /// Обработка закрытия по кнопке "Да" пользователем диалогового окна удаления элемента из таблицы.
    /// </summary>
    private async void DeleteDialogTopic_OnDeletedSuccessfully(bool isSuccessfullyDeleted)
    {
        if (isSuccessfullyDeleted)
        {
            // Вызов функции обновления из БД списка элементов, отображаемого на форме.
            await PopulateElementsAsync();
            // В таблице становится выбранным другой, определенный перед удалением.
            SelectedElement = SelectedElementAfterDelete;
            // Обновление данных на экране пользователя.
            StateHasChanged();
        }
    }

    /// <summary>
    /// Обработка нажатия пользователем кнопки "Вверх".
    /// </summary>
    private async void RecordMoveUp()
    {
        // Если пользователь выбрал элемент в списке, то 
        if (SelectedElement != null && Objects != null)
        {
            // Получение OrderNumber выбранного в списке элемента.
            int orderNumberOfSelectedElement = SelectedElement.OrderNumber;

            if (orderNumberOfSelectedElement > 1)
            {
                // Поиск элемента с OrderNumber меньше, чем у выбранного.
                TopicModel? previousElementInList = Objects
                    .Where(i => i.OrderNumber < orderNumberOfSelectedElement)
                    .OrderBy(i => i.OrderNumber)
                    .LastOrDefault();

                // Если удалось найти элемент с OrderNumber меньше, чем у выбранного, то
                if (previousElementInList != null)
                {
                    // Сохранение значения OrderNumber у найденного элемента.
                    int orderNumberOfPreviousElementInList = previousElementInList.OrderNumber;
                    // Обмен значениями OrderNumber.
                    previousElementInList.OrderNumber = orderNumberOfSelectedElement;
                    SelectedElement.OrderNumber = orderNumberOfPreviousElementInList;
                    // Получение имени залогиненного пользователя.
                    string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
                    // Сохранение изменений в элементах.
                    WebApiCallResult<TopicModel> webApiCallResultUpdatedElement = await ObjectDataService.UpdateObjectAsync(SelectedElement, currentUserName);
                    
                    // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                    if (webApiCallResultUpdatedElement.HasErrors)
                    {
                        // Вывод пользователю информации об этом.
                        HasWebApiErrors = webApiCallResultUpdatedElement.HasErrors;
                        WebApiErrorsDescription = webApiCallResultUpdatedElement.ResultDescription;
                        // Дальнейшее выполнение функции прекращается.
                        return;
                    }

                    WebApiCallResult<TopicModel> webApiCallResultPrev = await ObjectDataService.UpdateObjectAsync(previousElementInList, currentUserName);

                    // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                    if (webApiCallResultPrev.HasErrors)
                    {
                        // Вывод пользователю информации об этом.
                        HasWebApiErrors = webApiCallResultPrev.HasErrors;
                        WebApiErrorsDescription = webApiCallResultPrev.ResultDescription;
                        // Дальнейшее выполнение функции прекращается.
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Обработка нажатия пользователем кнопки "Вниз".
    /// </summary>
    private async void RecordMoveDown()
    {
        // Если пользователь выбрал элемент в списке, то 
        if (SelectedElement != null && Objects != null)
        {
            // Получение OrderNumber выбранного в списке элемента.
            int orderNumberOfSelectedElement = SelectedElement.OrderNumber;

            // Поиск элемента с OrderNumber больше, чем у выбранного.
            TopicModel? nextElementInList = Objects
                .Where(i => i.OrderNumber > orderNumberOfSelectedElement)
                .OrderBy(i => i.OrderNumber)
                .FirstOrDefault();

            // Если удалось найти элемент с OrderNumber больше, чем у выбранного, то
            if (nextElementInList != null)
            {
                // Сохранение значения OrderNumber у найденного элемента.
                int orderNumberOfNextElementInList = nextElementInList.OrderNumber;
                // Обмен значениями OrderNumber.
                nextElementInList.OrderNumber = orderNumberOfSelectedElement;
                SelectedElement.OrderNumber = orderNumberOfNextElementInList;
                // Получение имени залогиненного пользователя.
                string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
                // Сохранение изменений в элементах.
                WebApiCallResult<TopicModel> webApiCallResultUpdatedElement = await ObjectDataService.UpdateObjectAsync(SelectedElement, currentUserName);

                // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                if (webApiCallResultUpdatedElement.HasErrors)
                {
                    // Вывод пользователю информации об этом.
                    HasWebApiErrors = webApiCallResultUpdatedElement.HasErrors;
                    WebApiErrorsDescription = webApiCallResultUpdatedElement.ResultDescription;
                    // Дальнейшее выполнение функции прекращается.
                    return;
                }

                WebApiCallResult<TopicModel> webApiCallResultNext = await ObjectDataService.UpdateObjectAsync(nextElementInList, currentUserName);

                // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
                if (webApiCallResultNext.HasErrors)
                {
                    // Вывод пользователю информации об этом.
                    HasWebApiErrors = webApiCallResultNext.HasErrors;
                    WebApiErrorsDescription = webApiCallResultNext.ResultDescription;
                    // Дальнейшее выполнение функции прекращается.
                    return;
                }
            }
        }
    }
}