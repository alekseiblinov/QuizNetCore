using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace quiz.Ui.Shared;

/// <summary>
/// Контрол для отображения в верхней части всех страниц. Выводится имя пользователя и кнопки управления "Вход", "Выход" и т.д.
/// </summary>
public partial class LoginDisplay
{
    /// <summary>
    /// Ссылка на сервис для выполнения перенаправления на адреса Url. Внедряется с помощью DI.
    /// </summary>
    [Inject]
#pragma warning disable CS8618
    private NavigationManager NavigationManagerInstance { get; set; }

    /// <summary>
    /// Объект предоставляет API для управления данными пользователей в подсистеме Identity.
    /// </summary>
    [Inject]
    private UserManager<IdentityUser> UserManager { get; set; }
    
    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Для хранения списка ролей пользователя.
    /// </summary>
    private IList<string>? CurrentUserRoles { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Получение информации об активном залогиненном пользователе.
        ClaimsPrincipal userClaimsPrincipal = (await AuthenticationStateTask).User;
        IdentityUser userIdentity = await UserManager.GetUserAsync(userClaimsPrincipal);

        // Если удалось получить информацию об активном залогиненном пользователе, то
        if (userIdentity != null)
        {
            // Получение информации о ролях, в которые входит активный залогиненный пользователь.
            CurrentUserRoles = await UserManager.GetRolesAsync(userIdentity);
        }
    }

    /// <summary>
    /// Переход на страницу регистрации.
    /// </summary>
    private void NavigateToRegisterPage()
    {
        // Переход к странице входа.
        NavigationManagerInstance.NavigateTo($"/Identity/Account/Register?returnUrl={HttpUtility.UrlEncode(NavigationManagerInstance.ToBaseRelativePath(NavigationManagerInstance.Uri))}", true);
    }    
    
    /// <summary>
    /// Переход на страницу входа.
    /// </summary>
    private void NavigateToLoginPage()
    {
        // Переход к странице входа.
        NavigationManagerInstance.NavigateTo($"/Identity/Account/Login?returnUrl={HttpUtility.UrlEncode(NavigationManagerInstance.ToBaseRelativePath(NavigationManagerInstance.Uri))}", true);
    }

    /// <summary>
    /// Переход на страницу выхода.
    /// </summary>
    private void NavigateToLogoutPage()
    {
        // Переход к странице входа.
        NavigationManagerInstance.NavigateTo("/Identity/Account/LogOut", true);
    }
}