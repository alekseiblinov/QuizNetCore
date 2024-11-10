using Microsoft.AspNetCore.Mvc;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления данными записей Лога.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SystemTestController : Controller
{
    /// <summary>
    /// Объект сервиса для управления сообщениями лога.
    /// </summary>
    //private readonly ILog _logService;

    public SystemTestController()
    {
    }        
        
    //public SystemTestController(ILog logService)
    //{
    //    _logService = logService;
    //}

    /// <summary>
    /// Получение списка всех объектов.
    /// </summary>
    [Route("GetSystemState")]
    [HttpGet]
    public IActionResult GetSystemState()
    {
        // Возвращение запрошенных данных.
        object requestResult = "System state is Ok.";
        //_logService.WriteLineAsync("webapi: Вызвван метод GetSystemState").Wait();

        return Ok(requestResult);
    }
}