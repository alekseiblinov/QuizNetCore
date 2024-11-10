using quiz.Logger.Repositories;
using quiz.ModelBusiness;
using quiz.ModelDb;
using quiz.Shared;

namespace quiz.Api.Repositories;

/// <summary>
/// Репозиторий для управления аутентификацией и авторизацией пользователей.
/// *** Для ускорения работы лучше обойтись без использования Entity Framework и реализовать обращение к БД напрямую с помощью ADO.NET.
/// </summary>
public class SecurityRepository : ISecurityRepository
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
    /// Объект репозитория для управления данными Лога в БД.
    /// </summary>
    private readonly ILogRepository _logRepository;

    public SecurityRepository(quizContext quizContext, IConfiguration configuration, ILogRepository logRepository)
    {
        _appDbContext = quizContext;
        _configuration = configuration;
        _logRepository = logRepository;
    }
    
    /// <summary>
    /// Проверка действия указанного Токена безопасности.
    /// </summary>
    public bool IsSecurityTokenValid(string? clientSecurityTokenId, string? clientCurrentIpAddress, string? callerMethodFullName = null, string? methodParametersHash = null)
    {
        // *** Для ускорения можно перенести логику проверки Токена в хранимую процедуру с with (nolock) и здесь вызывать её вместо использования EntityFramework.
        bool result = false;

        // Если Id токена передано и это Guid, то
        if (clientSecurityTokenId != null && Guid.TryParse(clientSecurityTokenId, out Guid clientSecurityTokenIdGuid))
        {
            // Получение из БД информации об указанном Токене безопасности.
            SecurityToken? token = _appDbContext.SecurityTokens.ToList().FirstOrDefault(i => i.Id == clientSecurityTokenIdGuid);

            // Если в БД присутствует информация о Токене безопасности с указанным Id, то 
            if (token != null)
            {
                // Если информация о вызывающем коде не передана, то
                if (string.IsNullOrWhiteSpace(callerMethodFullName))
                {
                    // Получение имени вызывающего класса и метода.
                    string? callerClassName = CommonLogic.GetStackTraceClassName(2)?.RemoveSuffix("Controller");
                    string? callerMethodName = CommonLogic.GetStackTraceMethodName(2);
                    callerMethodFullName = $"{callerClassName}_{callerMethodName}";
                }

                // Получение из настроек значения времени жизни Токена безопасности из настроек.
                int securityTokenLifetimeSeconds = int.Parse(_configuration["SecurityTokenLifetimeSeconds"]);

                // Если срок действия Токена не истек, то
                if (token.CreatedAt >= DateTime.Now.AddSeconds(-securityTokenLifetimeSeconds))
                {
                    // Если имя метода, для вызова которого выдавался токен, совпадает с именем метода, заявленного для проверки, то
                    if (token.MethodName == callerMethodFullName)
                    {
                        // Если Токен не был использован ранее, то
                        if (token.IsUsed == false)
                        {
                            // *** В качестве параноидального режима можно включить проверку совпадения хэша переданных параметров.
                            // if (editingEntity.MethodParametersHash == methodParametersHash) { ... }
                            
                            // Если IP-адрес передан, то
                            if (!string.IsNullOrWhiteSpace(clientCurrentIpAddress))
                            {
                                // Получение из настроек значения времени в минутах, в течение которого проверяются прошлые IP-адреса, с которых были зарегистрированы Токены безопасности пользователя с таким именем.
                                int securityTokenRevokeTimeSpanMinutes = int.Parse(_configuration["SecurityTokenIpAddressesCheckDurationMinutes"]);
                                // Получение списка IP-адресов, с которых за последние 15 минут были проверки Токенов этого пользователя.
                                List<SecurityToken> userEarlierTokens = _appDbContext.SecurityTokens.Where(
                                            i => i.UserName == token.UserName 
                                            && i.CreatedAt > DateTime.Now.AddMinutes(-securityTokenRevokeTimeSpanMinutes) 
                                            && i.Id != clientSecurityTokenIdGuid 
                                            && i.IsUsed == true
                                         ).ToList();

                                // Если среди зарегистрированных за последние 15 минут Токенов пользователя с известным IP-адресом не существуют Токены, зарегистрированные с другого IP-адреса, то
                                if (userEarlierTokens
                                    .Where(i => i.IpAddress != null)
                                    .All(i => i.IpAddress == clientCurrentIpAddress))
                                {
                                    // Результат проверки положительный.
                                    result = true;
                                }
                                else
                                {
                                    // Запись в лог информации о подозрительном явлении.
                                    _logRepository.AddLogRecordAsync(new LogRecordModel(Guid.NewGuid(), $"modelbusiness security: В процессе проверки Токена безопасности обнаружено подозрительное наличие различных IP-адресов у того же пользователя за последние 15 минут. ID Токена: '{token.Id}'. EditingEntity.IpAddress: {token.IpAddress}. ClientCurrentIpAddress: {clientCurrentIpAddress}.", DateTime.Now));
                                }
                            }
                            else
                            {
                                // Результат проверки положительный.
                                result = true;
                            }
                        }
                        else
                        {
                            // Запись в лог информации о подозрительном явлении.
                            _logRepository.AddLogRecordAsync(new LogRecordModel(Guid.NewGuid(), $"modelbusiness security: В процессе проверки Токена безопасности обнаружена подозрительная попытка повторно проверить использованный ранее Токен. ID Токена: '{token.Id}'.", DateTime.Now));
                        }
                    }
                    else
                    {
                        // Запись в лог информации о подозрительном явлении.
                        _logRepository.AddLogRecordAsync(new LogRecordModel(Guid.NewGuid(), $"modelbusiness security: В процессе проверки Токена безопасности обнаружено подозрительное несовпадение имён методов. EditingEntity.MethodName: {token.MethodName}. MethodName: {callerMethodFullName}. ID Токена: '{token.Id}'.", DateTime.Now));
                    }
                }
                else
                {
                    // Запись в лог информации о подозрительном явлении.
                    _logRepository.AddLogRecordAsync(new LogRecordModel(Guid.NewGuid(), $"modelbusiness security: В процессе проверки Токена безопасности обнаружено истечение срока действия Токена. ID Токена: '{token.Id}'. Время создания токена: {token.CreatedAt}. Время проверки: {DateTime.Now}", DateTime.Now));
                }

                // Установка IP-адреса для Токена.
                if (!string.IsNullOrWhiteSpace(clientCurrentIpAddress))
                {
                    token.IpAddress = clientCurrentIpAddress;
                }

                // Установка для Токена метки, обозначающей что он был использован.
                token.IsUsed = true;
                // Сохранение изменений данных токена в БД.
                _appDbContext.SaveChanges();
            }
        }

        return result;
    }        
}