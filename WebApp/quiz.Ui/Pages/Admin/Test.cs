using quiz.ModelBusiness;
using quiz.Ui.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace quiz.Ui.Pages.Admin;

public partial class Test
{
// Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Ссылка на сервис для управления аутентификацией и авторизацией. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private ISecurityTokenManageDbDirect SecurityServiceInstance { get; set; }

    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618
    
    private async Task GetSecurityTokenAsync()
    {
        Guid tokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<TopicModel>((await AuthenticationStateTask).User.Identity?.Name);
    }

    private async Task RevokeSecurityTokenAsync()
    {
        // Вызов функции получения данных залогиненного пользователя.
        var userIdentity = (await AuthenticationStateTask).User.Identity;

        // Еслиу удалось успешно получить данные залогиненного пользователя, то
        if (userIdentity != null && userIdentity.Name != null)
        {
            // Вызов функции отзыва всех Токенов безопасности пользователя с указанным именем.
            await SecurityServiceInstance.RevokeUserSecurityTokensAsync(userIdentity.Name);
        }
    }
}