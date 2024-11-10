using quiz.Api.Repositories;
using quiz.Logger;
using quiz.ModelBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace quiz.Api.Controllers;

/// <summary>
/// Контроллер для управления Темами.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[SecurityTokenValidationFilter]
public class TopicController : Controller
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
    /// Объект репозитория для управления данными Тем в БД.
    /// </summary>
    private readonly IObjectRepository<TopicModel> _objectRepository;

    public TopicController(ILogDbDirect logService, IObjectRepository<TopicModel> objectRepository, ISecurityRepository securityRepository)
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
        TopicModel? result = _objectRepository.GetObjectById(id);

        return result != null ? 
            Ok(result) : 
            NotFound();
    }

    /// <summary>
    /// Создание нового объекта.
    /// </summary>
    [HttpPost]
    public IActionResult AddObject([FromBody] TopicModel objectModel)
    {
        _logService.WriteLineAsync($"webapi: Создание нового объекта Темы '{objectModel.Name}'.").Wait();

        if (objectModel.Name == string.Empty)
        {
            ModelState.AddModelError("Topic", "The Name shouldn't be empty");
        }

        if (objectModel.OrderNumber == 0)
        {
            ModelState.AddModelError("Topic", "The OrderNumber shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        TopicModel createdObject = _objectRepository.AddObject(objectModel);

        return Created("Topic", createdObject);
    }

    /// <summary>
    /// Обновление данных объекта.
    /// </summary>
    [HttpPut]
    public IActionResult UpdateObject([FromBody] TopicModel objectModel)
    {
        _logService.WriteLineAsync($"webapi: Обновление данных объекта Темы с id='{objectModel.Id}'.").Wait();

        if (objectModel.Name == string.Empty)
        {
            ModelState.AddModelError("Topic", "The Name shouldn't be empty");
        }

        if (objectModel.OrderNumber == 0)
        {
            ModelState.AddModelError("Topic", "The OrderNumber shouldn't be empty");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        TopicModel? objectToUpdate = _objectRepository.GetObjectById(objectModel.Id);

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
        _logService.WriteLineAsync($"webapi: Удаление данных объекта Темы с id='{id}'.").Wait();

        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        TopicModel? objectToDelete = _objectRepository.GetObjectById(id);

        if (objectToDelete == null)
        {
            return NotFound();
        }

        _objectRepository.DeleteObject(id);

        return NoContent();
    }
}