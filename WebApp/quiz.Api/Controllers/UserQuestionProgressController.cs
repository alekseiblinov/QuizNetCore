using quiz.Api.Repositories;
using quiz.Logger;
using quiz.ModelBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления данными об Ответах пользователей.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[SecurityTokenValidationFilter]
public class UserQuestionProgressController : Controller
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
    /// Объект репозитория для управления данными Ответов пользователей в БД.
    /// </summary>
    private readonly IObjectRepository<UserQuestionProgressModel> _objectRepository;

    public UserQuestionProgressController(ILogDbDirect logService, IObjectRepository<UserQuestionProgressModel> objectRepository, ISecurityRepository securityRepository)
    {
        _logService = logService;
        _objectRepository = objectRepository;
        _securityRepository = securityRepository;
    }

    /// <summary>
    /// Получение списка всех объектов.
    /// </summary>
    [Route("GetAllObjects")]
    [HttpGet]
    public IActionResult GetAllObjects()
    {
        // Возвращение запрошенных данных.
        return Ok(_objectRepository.GetAllObjects());
    }

    /// <summary>
    /// Получение данных объекта по его Id.
    /// </summary>
    [Route("GetObjectById/{id}")]
    [HttpGet]
    public IActionResult GetObjectById(Guid id)
    {
        // Возвращение запрошенных данных.
        UserQuestionProgressModel? result = _objectRepository.GetObjectById(id);

        return result != null ? 
            Ok(result) : 
            NotFound();
    }

    /// <summary>
    /// Создание нового объекта.
    /// </summary>
    [HttpPost]
    public IActionResult AddObject([FromBody] UserQuestionProgressModel objectModel)
    {
        _logService.WriteLineAsync($"webapi: Создание нового объекта Ответа пользователя '{objectModel.Id}'.").Wait();

        if (objectModel.QuestionId == Guid.Empty)
        {
            ModelState.AddModelError("UserQuestionProgress", "The QuestionId shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        UserQuestionProgressModel createdObject = _objectRepository.AddObject(objectModel);

        return Created("UserQuestionProgress", createdObject);
    }

    /// <summary>
    /// Обновление данных объекта.
    /// </summary>
    [HttpPut]
    public IActionResult UpdateObject([FromBody] UserQuestionProgressModel objectModel)
    {
        _logService.WriteLineAsync($"webapi: Обновление данных объекта Ответа пользователя с id='{objectModel.Id}'.").Wait();

        if (objectModel.QuestionId == Guid.Empty)
        {
            ModelState.AddModelError("UserQuestionProgress", "The QuestionId shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        UserQuestionProgressModel? objectToUpdate = _objectRepository.GetObjectById(objectModel.Id);

        if (objectToUpdate == null)
        {
            return NotFound();
        }

        _objectRepository.UpdateObject(objectModel);

        return NoContent(); 
    }

    /// <summary>
    /// Удаление объекта.
    /// </summary>
    [Route("DeleteObject/{id}")]
    [HttpDelete]
    public IActionResult DeleteObject(Guid id)
    {
        _logService.WriteLineAsync($"webapi: Удаление данных объекта Ответ пользователя с id='{id}'.").Wait();

        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        UserQuestionProgressModel? objectToDelete = _objectRepository.GetObjectById(id);

        if (objectToDelete == null)
        {
            return NotFound();
        }

        _objectRepository.DeleteObject(id);

        return NoContent();
    }
}