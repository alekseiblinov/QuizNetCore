namespace quiz.Api.Repositories;

/// <summary>
/// Интерфейс репозитория для управления аутентификацией и авторизацией пользователей.
/// </summary>
public interface ISecurityRepository
{
    /// <summary>
    /// Проверка действия указанного Токена безопасности.
    /// </summary>
    public bool IsSecurityTokenValid(string? clientSecurityTokenId, string? clientCurrentIpAddress, string? callerMethodFullName = null, string? methodParametersHash = null);
}