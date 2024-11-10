using quiz.Api.Repositories;
using quiz.Logger.Repositories;
using quiz.ModelBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления данными записей Лога.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LogController : Controller
{
    /// <summary>
    /// Объект репозитория для управления данными Лога в БД.
    /// </summary>
    private readonly ILogRepository _logRepository;

    /// <summary>
    /// Объект сервиса для управления данными авторизацией и аутентификацией пользователей в БД.
    /// </summary>
    private readonly ISecurityRepository _securityRepository;

    public LogController(ILogRepository logRepository, ISecurityRepository securityRepository)
    {
        _logRepository = logRepository;
        _securityRepository = securityRepository;
    }

    /// <summary>
    /// Получение списка всех объектов.
    /// </summary>
    [Route("GetAllLogRecords")]
    [HttpGet]
    public IActionResult GetAllLogRecords()
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        // Возвращение запрошенных данных.
        object requestResult = _logRepository.GetAllLogRecords();
        return Ok(requestResult);
    }

    /// <summary>
    /// Получение данных объекта по его Id.
    /// </summary>
    [Route("GetLogRecordById/{id}")]
    [HttpGet]
    public IActionResult GetLogRecordById(Guid id)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        // Возвращение запрошенных данных.
        LogRecordModel result = _logRepository.GetLogRecordById(id);

        return result != null ? 
            Ok(result) : 
            NotFound();
    }

    /// <summary>
    /// Создание нового объекта.
    /// </summary>
    [HttpPost]
    public IActionResult CreateLogRecord([FromBody] LogRecordModel objectModel)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        if (objectModel.Message == string.Empty)
        {
            ModelState.AddModelError("LogRecord", "The log message shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            
        LogRecordModel createdLogRecord = _logRepository.AddLogRecordAsync(objectModel).Result;

        return Created("LogRecord", createdLogRecord);
    }
}