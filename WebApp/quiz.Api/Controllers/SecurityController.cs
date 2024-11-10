using quiz.Api.Repositories;
using quiz.Logger;
using Microsoft.AspNetCore.Mvc;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления авторизацией и аутентификацией пользователей.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SecurityController : Controller
{
    /// <summary>
    /// Объект сервиса для управления сообщениями лога.
    /// </summary>
    private readonly ILogDbDirect _logService;

    /// <summary>
    /// Объект сервиса для управления данными авторизацией и аутентификацией пользователей в БД.
    /// </summary>
    private readonly ISecurityRepository _securityRepository;

    public SecurityController(ILogDbDirect logService, ISecurityRepository securityRepository)
    {
        _logService = logService;
        _securityRepository = securityRepository;
    }
    
    /// <summary>
    /// Проверка действия указанного Токена безопасности.
    /// *** Безопасней сделать с использованием метода POST для избежания передачи Id Токена безопасности в Url.
    /// </summary>
    [Route("IsSecurityTokenValid")]
    [HttpGet]
    public IActionResult IsSecurityTokenValid(Guid securityTokenId)
    {
        _logService.WriteLineAsync($"webapi: Проверка Токена безопасности '{securityTokenId}'.").Wait();
        bool result = false;

        try
        {
            // Установка значения ip-адреса исходного Токена безопасности.
            string? clientIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            // Вызов функции проверки действия указанного Токена безопасности.
            result = _securityRepository.IsSecurityTokenValid(securityTokenId.ToString(), clientIpAddress);
        }
        catch (Exception ex)
        {
            // Запись в лог информации об ошибке.
            _logService.WriteLineAsync($"webapi: Произошла ошибка в процессе проверки Токена безопасности '{securityTokenId}'. Ошибка: {ex}.").Wait();
        }
            
        return Ok(result);
    }
}