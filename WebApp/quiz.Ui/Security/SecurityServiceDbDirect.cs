using quiz.Logger;
using quiz.ModelBusiness;
using quiz.ModelDb;
using quiz.Shared;

namespace quiz.Ui.Security;

/// <summary>
/// Данные и методы для управления аутентификацией и авторизацией. В том числе контейнер состояний подсистемы аутентификации и авторизации.
/// По материалам https://chrissainty.com/3-ways-to-communicate-between-components-in-blazor/ (раздел 3. State Container).
/// </summary>
public class SecurityServiceDbDirect : ISecurityTokenManageDbDirect
{
    /// <summary>
    /// Ссылка на EntityFramework-контекст базы данных.
    /// </summary>
    private readonly quizContext _appDbContext;
    
    /// <summary>
    /// Объект с данными настроек приложения.
    /// </summary>
    private IConfiguration _configuration { get; }

    /// <summary>
    /// Ссылка на сервис для взаимодействия с данными лога.
    /// </summary>
    private ILogDbDirect _logService { get; set; }

    public SecurityServiceDbDirect(quizContext quizContext, IConfiguration configuration, ILogDbDirect logService)
    {
        _appDbContext = quizContext;
        _configuration = configuration;
        _logService = logService;
    }

    /// <summary>
    /// Получение Id Токена безопасности текущего залогиненного пользователя для последующего однократного использования для обращения к сервисам данных.
    /// </summary>
    public async Task<Guid> GetSecurityTokenIdAsync<T>(string? userName, string? methodName = null)
    {
        // Генерация уникального значения. Пустое значение Guid.Empty генерировать небезопасно потому что но предсказуемо и может быть использовано для взлома.
        Guid result = Guid.NewGuid();

        // Если имя пользователя передано, то
        if (userName != null)
        {
            // Если имя метода, для которого нужно зарегистрировать Токен, не передано явно, то
            if (methodName == null)
            {
                // Получение имени вызывающего класса и метода из CallStack.
                string currentMethodTypeName = typeof(T).Name.RemoveSuffix("Model");
                string? callerMethodName = quiz.Shared.CommonLogic.GetStackTraceMethodName(5);
                
                // По неизвестным пока причинам иногда требуется брать 5-й фрейм из CallStack, а иногда 4-й. Поэтому если в 5-м фрейме не содержится нужное имя, то
                if (callerMethodName == "Start")
                {
                    // Корректировка значения - получение имени из 4-го фрейма.
                    callerMethodName = quiz.Shared.CommonLogic.GetStackTraceMethodName(4);
                }

                // Дополнительная корректировка значения - удаление лишних частей имени.
                callerMethodName = callerMethodName?.RemoveSuffix("Internal").RemoveSuffix("Async");
                // Формирование результата.
                methodName = $"{currentMethodTypeName}_{callerMethodName}";
            }

            // Формирование данных, описывающих обращение к методу.
            SecurityTokenModel securityTokenModel = new SecurityTokenModel(
                Guid.NewGuid(),
                userName,
                methodName,
                string.Empty);

            try
            {
                await _logService.WriteLineAsync($"frontend: Получение нового Токена безопасности для последующего однократного использования для обращения к сервисам данных. Имя пользователя: '{userName}'. Имя метода: '{methodName}'");

                // Формирование и сохранение записи с информацией о Токене безопасности.
                SecurityToken newEntity = new SecurityToken
                                          {
                                              Id = securityTokenModel.Id,
                                              UserName = securityTokenModel.UserName,
                                              MethodName = securityTokenModel.MethodName,
                                              MethodParametersHash = securityTokenModel.MethodParametersHash,
                                              IsUsed = false
                                          };
                _appDbContext.SecurityTokens.Add(newEntity);
                // Сохранение изменений данных токена в БД.
                await _appDbContext.SaveChangesAsync();

                result = newEntity.Id;
            }
            catch (Exception ex)
            {
                // Запись в лог информации об ошибке.
                await _logService.WriteLineAsync($"frontend: Произошла ошибка в процессе получения нового Токена безопасности для последующего однократного использования для обращения к сервисам данных. Имя пользователя: '{userName}'. Имя метода: '{methodName}'. Ошибка: {ex}.");
            }
        }

        return result;
    }

    /// <summary>
    /// Отзыв всех Токенов безопасности пользователя с указанным именем, чтобы сделать невозможным их использование.
    /// </summary>
    public async Task RevokeUserSecurityTokensAsync(string userName)
    {
        try
        {
            await _logService.WriteLineAsync($"frontend: Отзыв Токена безопасности для пользователя '{userName}'.");
            // Получение из настроек значения времени в минутах, в течение которого из прошлого могут быть отозваны Токены безопасности.
            int securityTokenRevokeTimeSpanMinutes = int.Parse(_configuration["SecurityTokenRevokeTimeSpanMinutes"]);
            // Получение всех неиспользованных токенов пользователя, созданных за последние SecurityTokenRevokeTimeSpanMinutes минут.
            var userTokens = _appDbContext.SecurityTokens.Where(i => i.UserName == userName && i.CreatedAt > DateTime.Now.AddMinutes(-securityTokenRevokeTimeSpanMinutes) && i.IsUsed == false);

            // Перебор всех найденных Токенов.
            foreach (var currentUserToken in userTokens)
            {
                // Пометка Токена как использованного (отзыв его).
                currentUserToken.IsUsed = true;
            }

            // Сохранение информации в БД.
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Запись в лог информации об ошибке.
            await _logService.WriteLineAsync($"frontend: Произошла ошибка в процессе отзыва Токена безопасности для пользователя '{userName}'. Ошибка: {ex}.");
        }
    }
}