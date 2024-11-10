using System.Web;
using quiz.Logger;
using quiz.ModelBusiness;
using quiz.Ui.Components;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace quiz.Ui.Areas.Identity.Pages.Account;

/// <summary>
/// Контрол для просмотра объектов Пользователей.
/// </summary>
public partial class UsersOverview
{
// Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Ссылка на сервис для выполнения перенаправления на адреса Url. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManagerInstance { get; set; }

    /// <summary>
    /// Ссылка на сервис для взаимодействия с данными лога. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private ILogDbDirect LogService { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных объектов. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private IObjectDataService<UserModel> ObjectDataService { get; set; }

    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Список отображаемых в таблице элементов.
    /// </summary>
    private IEnumerable<UserModel>? Objects { get; set; }

    /// <summary>
    /// Диалог для удаления элемента.
    /// </summary>
    private DialogDelete ObjectDeleteDialog { get; set; } = null!;

    /// <summary>
    /// Данные выбранного в таблице элемента.
    /// </summary>
    private UserModel? SelectedElement { get; set; }

    /// <summary>
    /// Данные элемента таблицы, который будет выбран после завершения удаления элемента.
    /// </summary>
    private UserModel? SelectedElementAfterDelete { get; set; }

    /// <summary>
    /// Недоступность кнопки "Удалить".
    /// </summary>
    private bool IsDeleteButtonDisabled => false;

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
        // Получение данных элементов списка через WebAPI.
        WebApiCallResult<IEnumerable<UserModel>> webApiCallResult = await ObjectDataService.GetAllObjectsAsync(currentUserName);

        // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
        if (webApiCallResult.HasErrors)
        {
            // Вывод пользователю информации об этом.
            HasWebApiErrors = webApiCallResult.HasErrors;
            WebApiErrorsDescription = webApiCallResult.ResultDescription;
            // Дальнейшее выполнение функции прекращается.
            return;
        }

        // Обновление списка элементов на экране.
        Objects = webApiCallResult.Data?.OrderByDescending(i => i.Login);
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
    private void UserAdd()
    {
        // Переход к странице редактирования Пользователей.
        NavigationManagerInstance.NavigateTo($"Identity/Account/useredit/{HttpUtility.UrlEncode(NavigationManagerInstance.Uri)}");
    }           
        
    /// <summary>
    /// Обработка нажатия пользователем на кнопку "Редактировать".
    /// </summary>
    private void UserEdit(Guid userId)
    {
        // Переход к странице редактирования Пользователей.
        NavigationManagerInstance.NavigateTo($"Identity/Account/useredit/{userId}/{HttpUtility.UrlEncode(NavigationManagerInstance.Uri)}");
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
            List<UserModel> sandwichedItems = Objects.FindSandwichedItem(item => item.Id == SelectedElement.Id).ToList();
            UserModel previousItem = sandwichedItems[0];
            //var deletedItem = sandwichedItems[1];
            UserModel nextItem = sandwichedItems[2];

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
    private async void DeleteDialogUser_OnDeletedSuccessfully(bool isSuccessfullyDeleted)
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
    /// Получение в виде единой строки списка ролей пользователя по его Id.
    /// </summary>
    private string GetUsersRolesStringById(List<RoleModel> roles)
    {
        string result = string.Empty;

        // Перебор ролей пользователя с указанным Id и составление строки с перечислением его ролей.
        foreach (var currentUserRole in roles)
        {
            result = !string.IsNullOrWhiteSpace(result) 
                ? $"{result}; {currentUserRole.Name}" 
                : currentUserRole.Name;
        }

        return result;
    }
}