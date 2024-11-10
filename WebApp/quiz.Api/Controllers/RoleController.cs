using quiz.Api.Repositories;
using quiz.Logger;
using quiz.ModelBusiness;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления Ролями.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RoleController : Controller
{
    /// <summary>
    /// Объект сервиса для управления сообщениями лога.
    /// </summary>
    private readonly ILogDbDirect _logService;

    /// <summary>
    /// Объект сервиса для управления данными авторизацией и аутентификацией пользователей в БД.
    /// </summary>
    private readonly ISecurityRepository _securityRepository;

    /// <summary>
    /// Объект репозитория для управления данными Пользователей в БД.
    /// </summary>
    private readonly IObjectRepository<RoleModel> _objectRepository;

    private RoleManager<IdentityRole> _roleManager { get; set; }

    public RoleController(
        ILogDbDirect logService, 
        IObjectRepository<RoleModel> objectRepository, 
        ISecurityRepository securityRepository,
        RoleManager<IdentityRole> roleManager
        )
    {
        _logService = logService;
        _objectRepository = objectRepository;
        _securityRepository = securityRepository;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Получение списка всех объектов.
    /// </summary>
    [Route("GetAllObjects")]
    [HttpGet]
    public IActionResult GetAllObjects()
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        return Ok(_objectRepository.GetAllObjects());
    }

    /// <summary>
    /// Получение данных объекта по его Id.
    /// </summary>
    [Route("GetObjectById/{id}")]
    [HttpGet]
    public IActionResult GetObjectById(Guid id)
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
        RoleModel? result = _objectRepository.GetObjectById(id);

        return result != null ? 
            Ok(result) : 
            NotFound();
    }

    /// <summary>
    /// Создание нового объекта.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddObject([FromBody] RoleModel objectModel)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        _logService.WriteLineAsync($"webapi: Создание новой Роли с Id='{objectModel.Id}'.").Wait();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        IdentityRole newRole = new IdentityRole(objectModel.Name);
        var result = await _roleManager.CreateAsync(newRole);

        return Created("Role", objectModel);
    }

    /// <summary>
    /// Обновление данных объекта.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateObject([FromBody] RoleModel objectModel)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        _logService.WriteLineAsync($"webapi: Обновление данных Роли с id='{objectModel.Id}'.").Wait();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        RoleModel? controlLabelAndValueStringToUpdate = _objectRepository.GetObjectById(objectModel.Id);

        if (controlLabelAndValueStringToUpdate == null)
        {
            return NotFound();
        }

        IdentityRole role = await _roleManager.FindByIdAsync(objectModel.Id.ToString());

        if (role == null)
        {
            return NotFound();
        }

        role.Name = objectModel.Name;
        IdentityResult? result = await _roleManager.UpdateAsync(role);

        return NoContent();
    }

    /// <summary>
    /// Удаление объекта.
    /// </summary>
    [Route("DeleteObject/{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteObject(Guid id)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        _logService.WriteLineAsync($"webapi: Удаление данных Роли с id='{id}'.").Wait();

        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        RoleModel? roleToDelete = _objectRepository.GetObjectById(id);

        if (roleToDelete == null)
        {
            return NotFound();
        }

        IdentityRole role = await _roleManager.FindByIdAsync(id.ToString());

        if (role == null)
        {
            return NotFound();
        }

        IdentityResult? result = await _roleManager.DeleteAsync(role);

        return NoContent();
    }
}