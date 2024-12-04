using System.Web;
using quiz.Logger;
using quiz.ModelBusiness;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace quiz.Ui.Components;

/// <summary>
/// Контрол для просмотра списка Вопросов и кнопок управления для них.
/// </summary>
public partial class QuestionsOverview
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
    private IObjectDataService<QuestionModel> ObjectDataService { get; set; }

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
    private IEnumerable<QuestionModel>? Objects { get; set; }

    /// <summary>
    /// Диалог для удаления элемента.
    /// </summary>
    private DialogDelete ObjectDeleteDialog { get; set; } = null!;

    /// <summary>
    /// Данные выбранного в таблице элемента.
    /// </summary>
    private QuestionModel? SelectedItem
    {
        get => SelectedItemObject as QuestionModel;
        set => SelectedItemObject = value;
    }

    /// <summary>
    /// Данные выбранного в таблице элемента в виде объекта типа object. Использовать типизированный объект не позволяет ограничение компонента DxGrid.
    /// </summary>
    object? SelectedItemObject { get; set; }
        
    /// <summary>
    /// Данные элемента таблицы, который будет выбран после завершения удаления элемента.
    /// </summary>
    private QuestionModel? SelectedElementAfterDelete { get; set; }

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
        WebApiCallResult<IEnumerable<QuestionModel>> webApiCallResult = await ObjectDataService.GetAllObjectsAsync(currentUserName);

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
            Objects = webApiCallResult.Data.OrderBy(i => i.CreatedAt);
        }
    }

    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Добавить".
    /// </summary>
    private void QuestionAdd()
    {
        // Переход к странице редактирования Тем.
        NavigationManagerInstance.NavigateTo($"questionedit/{HttpUtility.UrlEncode(NavigationManagerInstance.Uri)}");
    }           
        
    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Редактировать".
    /// </summary>
    private void QuestionEdit(Guid id)
    {
        // Переход к странице редактирования классов Вопросов.
        string targetUrl = $"questionedit/{id}/{HttpUtility.UrlEncode(NavigationManagerInstance.Uri)}";
        NavigationManagerInstance.NavigateTo(targetUrl);
    }       

    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Удалить".
    /// </summary>
    private async void ObjectDeleteAsync()
    {
        // Если пользователь выбрал элемент в списке, то 
        if (SelectedItem != null && Objects != null)
        {
            // Получение элементов из списка, являющихся соседними к удаляемому.
            List<QuestionModel> sandwichedItems = Objects.FindSandwichedItem(item => item.Id == SelectedItem.Id).ToList();
            QuestionModel previousItem = sandwichedItems[0];
            //var deletedItem = sandwichedItems[1];
            QuestionModel nextItem = sandwichedItems[2];

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
    private async void DeleteDialogQuestion_OnDeletedSuccessfully(bool isSuccessfullyDeleted)
    {
        if (isSuccessfullyDeleted)
        {
            // Вызов функции обновления из БД списка элементов, отображаемого на форме.
            await PopulateElementsAsync();
            // В таблице становится выбранным другой, определенный перед удалением.
            SelectedItem = SelectedElementAfterDelete;
            // Обновление данных на экране пользователя.
            StateHasChanged();
        }
    }
}