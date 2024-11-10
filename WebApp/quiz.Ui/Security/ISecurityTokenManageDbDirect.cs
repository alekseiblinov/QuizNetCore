namespace quiz.Ui.Security;

/// <summary>
/// Методы управление доступом (Identity) с записью и чтением данных напрямую из DbContext.
/// </summary>
public interface ISecurityTokenManageDbDirect
{
    /// <summary>
    /// Получение Id Токена безопасности текущего залогиненного пользователя для последующего однократного использования для обращения к сервисам данных.
    /// </summary>
    Task<Guid> GetSecurityTokenIdAsync<T>(string? userName, string? methodName = null);

    /// <summary>
    /// Отзыв всех Токенов безопасности пользователя с указанным именем, чтобы сделать невозможным их использование.
    /// </summary>
    Task RevokeUserSecurityTokensAsync(string userName);
}