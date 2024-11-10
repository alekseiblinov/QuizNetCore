using System.Net;
using System.Text;
using System.Text.Json;
using quiz.Shared;
using quiz.Ui.Security;

namespace quiz.Ui.Services;

/// <summary>
/// Сервис для передачи в БД и получения из нее данных.
/// </summary>
public class ObjectDataService<T> : IObjectDataService<T>
{
    /// <summary>
    /// Сервис, содержащий логику взаимодействия с конечными точками по протоколу http(s).
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Ссылка на сервис для управления аутентификацией и авторизацией.
    /// </summary>
    private ISecurityTokenManageDbDirect SecurityServiceInstance { get; set; }

    public ObjectDataService(HttpClient httpClient, ISecurityTokenManageDbDirect securityServiceInstance)
    {
        _httpClient = httpClient;
        SecurityServiceInstance = securityServiceInstance;
    }

    /// <summary>
    /// Получить все объекты указанного типа от backend.
    /// </summary>
    public async Task<WebApiCallResult<IEnumerable<T>>> GetAllObjectsAsync(string? userName)
    {
        WebApiCallResult<IEnumerable<T>> result = new WebApiCallResult<IEnumerable<T>>();
        // Получение данных элементов списка через WebAPI.
        WebApiCallResultRaw<IEnumerable<T>> webApiCallResult = await GetAllObjectsAsyncInternal(userName);

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResult.IsSuccess && webApiCallResult.Data != null)
        {
            // Обновление списка элементов на экране.
            result.Data = (webApiCallResult.Data);
            // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = false;
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (webApiCallResult.IsUnauthorized)
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = "Ошибка при обращении к WebAPI: недостаточно прав доступа.";
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = $"Ошибка при обращении к WebAPI. Код {webApiCallResult.StatusCode}.";
        }

        return result;
    }

    /// <summary>
    /// Получить от backend данные объекта с указанным ID.
    /// </summary>
    public async Task<WebApiCallResult<T>> GetObjectByIdAsync(Guid id, string? userName)
    {
        WebApiCallResult<T> result = new WebApiCallResult<T>();
        // Получение данных элемента для редактирования через WebAPI.
        WebApiCallResultRaw<T?> webApiCallResult = await GetObjectByIdAsyncInternal(id, userName);

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResult.IsSuccess)
        {
            // Получение данных редактируемого объекта из результата обращения к WebAPI.
            result.Data = webApiCallResult.Data;
            // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = false;
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (webApiCallResult.IsUnauthorized)
        {
            // Установка признака, обозначающего наличие ошибок.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = "Ошибка при обращении к WebAPI: недостаточно прав доступа.";
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Установка признака, обозначающего наличие ошибок.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = $"Ошибка при обращении к WebAPI. Код {webApiCallResult.StatusCode}.";
        }

        return result;
    }

    /// <summary>
    /// Получить от backend данные объектов, относящихся к определенной группе с указанным ID.
    /// </summary>
    public async Task<WebApiCallResult<IEnumerable<T>>> GetObjectsInGroupAsync(Guid id, string? userName)
    {
        WebApiCallResult<IEnumerable<T>> result = new WebApiCallResult<IEnumerable<T>>();
        // Получение от backend списка элементов.
        WebApiCallResultRaw<IEnumerable<T>> webApiCallResult = await GetObjectsInGroupAsyncInternal(id, userName);

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResult.IsSuccess)
        {
            // Обновление списка элементов на экране.
            result.Data = webApiCallResult.Data;
            // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = false;
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (webApiCallResult.IsUnauthorized)
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = "Ошибка при обращении к WebAPI: недостаточно прав доступа.";
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = $"Ошибка при обращении к WebAPI. Код {webApiCallResult.StatusCode}.";
        }

        return result;
    }

    /// <summary>
    /// Добавить новый объект в БД.
    /// </summary>
    public async Task<WebApiCallResult<T>> AddObjectAsync(T objectModel, string? userName)
    {
        WebApiCallResult<T> result = new WebApiCallResult<T>();

        // Вызов функции добавления данных нового объекта в БД.
        WebApiCallResultRaw<T?> webApiCallResult = await AddObjectAsyncInternal(objectModel, userName);

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResult.IsSuccess && webApiCallResult.Data != null)
        {
            // Сброс признака, свидетельствующего о наличии ошибок.
            result.HasErrors = false;
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (webApiCallResult.IsUnauthorized)
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = "Ошибка при обращении к WebAPI: недостаточно прав доступа.";
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = $"Ошибка при обращении к WebAPI. Код {webApiCallResult.StatusCode}.";
        }

        return result;
    }

    /// <summary>
    /// Изменить (редактировать) данные объекта в БД.
    /// </summary>
    public async Task<WebApiCallResult<T>> UpdateObjectAsync(T objectModel, string? userName)
    {
        WebApiCallResult<T> result = new WebApiCallResult<T>();

        // Вызов функции сохранения данных объекта в БД.
        WebApiCallResultRaw<T?> webApiResult = await UpdateObjectAsyncInternal(objectModel, userName);

        // Если данные через WebAPI успешно получены, то 
        if (webApiResult.IsSuccess && webApiResult.Data != null)
        {
            // Сброс признака, свидетельствующего о наличии ошибок.
            result.HasErrors = false;
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (webApiResult.IsUnauthorized)
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = "Ошибка при обращении к WebAPI: недостаточно прав доступа.";
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Установка признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
            result.HasErrors = true;
            // Установка текста сообщения для отображения пользователю.
            result.ResultDescription = $"Ошибка при обращении к WebAPI. Код {webApiResult.StatusCode}.";
        }	

        return result;
    }

    /// <summary>
    /// Удалить от backend элемент с указанным ID.
    /// </summary>
    public async Task<WebApiCallResultRaw<bool>> DeleteObjectAsync(Guid objectId, string? userName)
    {
        WebApiCallResultRaw<bool> result = new WebApiCallResultRaw<bool>(false, false, 0, default);

        // Получение Id Токена безопасности залогиненного пользователя.
        Guid securityTokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<T>(userName);
        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"api/{typeof(T).Name.RemoveSuffix("Model")}/DeleteObject/{objectId}");
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось успешно получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Присвоение полученного результата выходному значению.
            result = new WebApiCallResultRaw<bool>(true, false, (int)response.StatusCode, true);
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Формирование объекта с данными результата выполнения.
            result = new WebApiCallResultRaw<bool>(false, true, (int)response.StatusCode, default);
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Формирование объекта с данными результата выполнения.
            result.StatusCode = (int)response.StatusCode;
        }

        return result;
    }

    /// <summary>
    /// Получить все объекты указанного типа от backend. Обращение непосредственно к WebAPI и обработка (распарсивание) получившегося результата.
    /// </summary>
    private async Task<WebApiCallResultRaw<IEnumerable<T>>> GetAllObjectsAsyncInternal(string? userName)
    {
        WebApiCallResultRaw<IEnumerable<T>> result = new WebApiCallResultRaw<IEnumerable<T>>(false, false, 0, null);

        // Получение Id Токена безопасности залогиненного пользователя.
        Guid securityTokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<T>(userName);
        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/{typeof(T).Name.RemoveSuffix("Model")}/GetAllObjects");
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось успешно получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Конвертация полученного из WebAPI результата в объект данных.
            IEnumerable<T>? responseDeserialized = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // Если удалось выполнить конвертацию полученного из WebAPI результата в объект данных, то
            if (responseDeserialized != null)
            {
                // Присвоение полученного результата выходному значению.
                result = new WebApiCallResultRaw<IEnumerable<T>>(true, false, (int)response.StatusCode, responseDeserialized);
            }
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Формирование объекта с данными результата выполнения.
            result = new WebApiCallResultRaw<IEnumerable<T>>(false, true, (int)response.StatusCode, null);
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Формирование объекта с данными результата выполнения.
            result.StatusCode = (int)response.StatusCode;
        }

        return result;
    }
    
    /// <summary>
    /// Получить от backend данные объекта с указанным ID. Обращение непосредственно к WebAPI и обработка (распарсивание) получившегося результата.
    /// </summary>
    private async Task<WebApiCallResultRaw<T?>> GetObjectByIdAsyncInternal(Guid objectId, string? userName)
    {
        WebApiCallResultRaw<T?> result = new WebApiCallResultRaw<T?>(false, false, 0, default);
        // Получение Id Токена безопасности залогиненного пользователя.
        Guid securityTokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<T>(userName);

        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/{typeof(T).Name.RemoveSuffix("Model")}/GetObjectById/{objectId}");
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось успешно получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Конвертация полученного из WebAPI результата в объект данных.
            T? responseDeserialized = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            // Присвоение полученного результата выходному значению.
            result = new WebApiCallResultRaw<T?>(true, false, (int)response.StatusCode, responseDeserialized);
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Формирование объекта с данными результата выполнения.
            result = new WebApiCallResultRaw<T?>(false, true, (int)response.StatusCode, default);
            // *** Здесь и других подобных местах после получения данных от WebAPI можно записывать информацию об ошибке в лог.
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            //// Получение данных об ошибках валидации на backend.
            //Dictionary<string, List<string>>? errors = await JsonSerializer.DeserializeAsync<Dictionary<string, List<string>>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            // Формирование объекта с данными результата выполнения.
            result.StatusCode = (int)response.StatusCode;
            // *** Здесь и других подобных местах после получения данных от WebAPI можно записывать информацию об ошибке в лог.
        }

        return result;
    }

    /// <summary>
    /// Получить от backend данные объектов, относящихся к определенной группе с указанным ID. Например, список всех Контролов LabelLine класса Страницы визита с указанным ID.
    /// </summary>
    private async Task<WebApiCallResultRaw<IEnumerable<T>>> GetObjectsInGroupAsyncInternal(Guid groupId, string? userName)
    {
        WebApiCallResultRaw<IEnumerable<T>> result = new WebApiCallResultRaw<IEnumerable<T>>(false, false, 0, null);

        // Получение Id Токена безопасности залогиненного пользователя.
        Guid securityTokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<T>(userName);
        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/{typeof(T).Name.RemoveSuffix("Model")}/GetObjectsInGroup/{groupId}");
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось успешно получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Конвертация полученного из WebAPI результата в объект данных.
            IEnumerable<T>? responseDeserialized = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // Если удалось выполнить конвертацию полученного из WebAPI результата в объект данных, то
            if (responseDeserialized != null)
            {
                // Присвоение полученного результата выходному значению.
                result = new WebApiCallResultRaw<IEnumerable<T>>(true, false, (int)response.StatusCode, responseDeserialized);
            }
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Формирование объекта с данными результата выполнения.
            result = new WebApiCallResultRaw<IEnumerable<T>>(false, true, (int)response.StatusCode, null);
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Формирование объекта с данными результата выполнения.
            result.StatusCode = (int)response.StatusCode;
        }
        
        return result;
    }

    /// <summary>
    /// Добавить новый объект в БД. Обращение непосредственно к WebAPI и обработка (распарсивание) получившегося результата.
    /// </summary>
    private async Task<WebApiCallResultRaw<T?>> AddObjectAsyncInternal(T objectModel, string? userName)
    {
        WebApiCallResultRaw<T?> result = new WebApiCallResultRaw<T?>(false, false, 0, default);

        // Получение Id Токена безопасности залогиненного пользователя.
        Guid securityTokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<T>(userName);
        // Подготовка объекта к передаче в WebAPI.
        StringContent objectModelJson = new StringContent(JsonSerializer.Serialize(objectModel), Encoding.UTF8, "application/json");

        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/{typeof(T).Name.RemoveSuffix("Model")}");
        request.Content = objectModelJson;
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось успешно получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Конвертация полученного из WebAPI результата в объект данных.
            T? responseDeserialized = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());
            // Присвоение полученного результата выходному значению.
            result = new WebApiCallResultRaw<T?>(true, false, (int)response.StatusCode, responseDeserialized);
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Формирование объекта с данными результата выполнения.
            result = new WebApiCallResultRaw<T?>(false, true, (int)response.StatusCode, default);
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            //// Получение данных об ошибках валидации на backend.
            //Dictionary<string, List<string>>? errors = await JsonSerializer.DeserializeAsync<Dictionary<string, List<string>>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            // Формирование объекта с данными результата выполнения.
            result.StatusCode = (int)response.StatusCode;
        }

        return result;
    }

    /// <summary>
    /// Изменить (редактировать) данные объекта в БД. Обращение непосредственно к WebAPI и обработка (распарсивание) получившегося результата.
    /// </summary>
    private async Task<WebApiCallResultRaw<T?>> UpdateObjectAsyncInternal(T objectModel, string? userName)
    {
        WebApiCallResultRaw<T?> result = new WebApiCallResultRaw<T?>(false, false, 0, default);

        // Получение Id Токена безопасности залогиненного пользователя.
        Guid securityTokenId = await SecurityServiceInstance.GetSecurityTokenIdAsync<T>(userName);
        // Подготовка объекта к передаче в WebAPI.
        StringContent objectModelJson = new StringContent(JsonSerializer.Serialize(objectModel), Encoding.UTF8, "application/json");
        
        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"api/{typeof(T).Name.RemoveSuffix("Model")}");
        request.Content = objectModelJson;
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось успешно получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Присвоение полученного результата выходному значению.
            result = new WebApiCallResultRaw<T?>(true, false, (int)response.StatusCode, objectModel);
        }
        // Если WebAPI сообщил о попытке неавторизованного доступа, то
        else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            // Формирование объекта с данными результата выполнения.
            result = new WebApiCallResultRaw<T?>(false, true, (int)response.StatusCode, default);
        }
        // Если WebAPI сообщил о непредвиденной ошибке при взаимодействии, то
        else
        {
            // Формирование объекта с данными результата выполнения.
            result.StatusCode = (int)response.StatusCode;
        }

        return result;
    }
}