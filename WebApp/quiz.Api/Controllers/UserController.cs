using quiz.Api.Repositories;
using quiz.Logger;
using quiz.ModelBusiness;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления Пользователями.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
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
    private readonly IObjectRepository<UserModel> _objectRepository;

    private UserManager<IdentityUser> UserManager { get; set; }

    public UserController(
        ILogDbDirect logService, 
        IObjectRepository<UserModel> objectRepository, 
        ISecurityRepository securityRepository,
        UserManager<IdentityUser> userManager
        )
    {
        _logService = logService;
        _objectRepository = objectRepository;
        _securityRepository = securityRepository;
        UserManager = userManager;
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
        UserModel? result = _objectRepository.GetObjectById(id);

        return result != null ? 
            Ok(result) : 
            NotFound();
    }

    /// <summary>
    /// Создание нового объекта.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddObject([FromBody] UserModel objectModel)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        _logService.WriteLineAsync($"webapi: Создание нового Пользователя с Id='{objectModel.Id}'.").Wait();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Формирование данных пользователя.
        IdentityUser newUser = new IdentityUser(objectModel.Login);
        newUser.UserName = objectModel.Login;
        newUser.Email = objectModel.Email;
        newUser.PasswordHash = UserManager.PasswordHasher.HashPassword(newUser, objectModel.Password);
        // Вызов сохранения данных пользователя в БД.
        IdentityResult result = await UserManager.CreateAsync(newUser);

        // Заполнение данных ролей пользователя.
        foreach (var currentUserRole in objectModel.Roles)
        {
            await UserManager.AddToRoleAsync(newUser, currentUserRole.Name.ToUpper());
        }

        return Created("User", objectModel);
    }

    /// <summary>
    /// Обновление данных объекта.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateObject([FromBody] UserModel objectModel)
    {
        // Получение значения Токена безопасности из заголовка запроса.
        Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);

        // Если Токен безопасности успешно прошел проверку, то
        if (!_securityRepository.IsSecurityTokenValid(securityTokenId, Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Возвращение соответствующей ошибки.
            return Unauthorized();
        }

        _logService.WriteLineAsync($"webapi: Обновление данных Пользователя с id='{objectModel.Id}'.").Wait();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        UserModel? controlLabelAndValueStringToUpdate = _objectRepository.GetObjectById(objectModel.Id);

        if (controlLabelAndValueStringToUpdate == null)
        {
            return NotFound();
        }

        IdentityUser user = await UserManager.FindByIdAsync(objectModel.Id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        // Формирование данных пользователя.
        user.UserName = objectModel.Login;
        user.Email = objectModel.Email;

        //Если значение нового пароля установлено, то
        if (!string.IsNullOrWhiteSpace(objectModel.Password))
        {
            // Смена пароля пользователя.
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, objectModel.Password);
        }

        // Заполнение данных ролей пользователя.
        List<string> roles = new List<string>();

        foreach (var currentUserRole in objectModel.Roles)
        {
            roles.Add(currentUserRole.Name);
        }

        // Получение списка ролей пользователя.
        var userRoles = await UserManager.GetRolesAsync(user);
        // Получаем список ролей, которые были добавлены для пользователя.
        var addedRoles = roles.Except(userRoles);
        // Получаем роли, которые были удалены для пользователя.
        var removedRoles = userRoles.Except(roles);
 
        // Вызов сохранения данных о ролях пользователя.
        await UserManager.AddToRolesAsync(user, addedRoles);
        await UserManager.RemoveFromRolesAsync(user, removedRoles);
        // Вызов сохранения данных пользователя в БД.
        IdentityResult result = await UserManager.UpdateAsync(user);

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

        _logService.WriteLineAsync($"webapi: Удаление данных Пользователя с id='{id}'.").Wait();

        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        UserModel? userToDelete = _objectRepository.GetObjectById(id);

        if (userToDelete == null)
        {
            return NotFound();
        }

        IdentityUser user = await UserManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        IdentityResult result = await UserManager.DeleteAsync(user);

        return NoContent();
    }
}