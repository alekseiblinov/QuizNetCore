using quiz.Api.Repositories;
using quiz.Logger;
using quiz.ModelBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления Вопросами.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[SecurityTokenValidationFilter]
public class QuestionController : Controller
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
    /// Объект репозитория для управления данными Вопросов в БД.
    /// </summary>
    private readonly IObjectRepository<QuestionModel> _objectRepository;

    public QuestionController(ILogDbDirect logService, IObjectRepository<QuestionModel> objectRepository, ISecurityRepository securityRepository)
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
        QuestionModel? result = _objectRepository.GetObjectById(id);

        return result != null ? 
            Ok(result) : 
            NotFound();
    }

    /// <summary>
    /// Создание нового объекта.
    /// </summary>
    [HttpPost]
    public IActionResult AddObject([FromBody] QuestionModel objectModel)
    {
        _logService.WriteLineAsync($"webapi: Создание нового объекта Вопрос '{objectModel.QuestionText}'.").Wait();

        if (objectModel.TopicId == Guid.Empty)
        {
            ModelState.AddModelError("Question", "The TopicId shouldn't be empty");
        }

        if (objectModel.QuestionText == string.Empty)
        {
            ModelState.AddModelError("Question", "The QuestionText shouldn't be empty");
        }

        if (objectModel.Option01 == string.Empty)
        {
            ModelState.AddModelError("Question", "The Option01 shouldn't be empty");
        }

        if (objectModel.Answer == string.Empty)
        {
            ModelState.AddModelError("Question", "The Answer shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        QuestionModel createdObject = _objectRepository.AddObject(objectModel);

        return Created("Question", createdObject);
    }

    /// <summary>
    /// Обновление данных объекта.
    /// </summary>
    [HttpPut]
    public IActionResult UpdateObject([FromBody] QuestionModel objectModel)
    {
        _logService.WriteLineAsync($"webapi: Обновление данных объекта Вопрос с id='{objectModel.Id}'.").Wait();

        if (objectModel.TopicId == Guid.Empty)
        {
            ModelState.AddModelError("Question", "The TopicId shouldn't be empty");
        }

        if (objectModel.QuestionText == string.Empty)
        {
            ModelState.AddModelError("Question", "The QuestionText shouldn't be empty");
        }

        if (objectModel.Option01 == string.Empty)
        {
            ModelState.AddModelError("Question", "The Option01 shouldn't be empty");
        }

        if (objectModel.Answer == string.Empty)
        {
            ModelState.AddModelError("Question", "The Answer shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        QuestionModel? objectToUpdate = _objectRepository.GetObjectById(objectModel.Id);

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
        _logService.WriteLineAsync($"webapi: Удаление данных объекта Вопрос с id='{id}'.").Wait();

        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        QuestionModel? objectToDelete = _objectRepository.GetObjectById(id);

        if (objectToDelete == null)
        {
            return NotFound();
        }

        _objectRepository.DeleteObject(id);

        return NoContent();
    }
}