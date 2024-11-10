using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using quiz.ModelBusiness;

namespace quiz.Logger;

/// <summary>
/// Сервис для передачи в БД и получения из нее данных записей Лога.
/// </summary>
public class LogServiceByHttp : ILogByHttp
{
    /// <summary>
    /// Сервис, содержащий логику взаимодействия с конечными точками по протоколу http(s).
    /// </summary>
    private readonly HttpClient _httpClient;

    public LogServiceByHttp(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Запись сообщения лога в БД.
    /// </summary>
    public async Task WriteLineAsync(string messageText, Guid securityTokenId)
    {
        LogRecordModel newRecord = new LogRecordModel(Guid.NewGuid(), messageText, null);
        // Подготовка объекта к передаче в WebAPI.
        StringContent logJson = new StringContent(JsonSerializer.Serialize(newRecord), Encoding.UTF8, "application/json");
        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/log");
        request.Content = logJson;
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI. Получение ответа ожидать не требуется.
        await _httpClient.SendAsync(request);

        // *** Здесь можно перехватывать ошибки логгирования. Например, можно записывать их в EventLog. С одной стороны они не должны оставаться незамеченными, с другой стороны не должны приводить к сбою всей системы.
    }

    /// <summary>
    /// Получить все записи лога из БД.
    /// </summary>
    public async Task<IEnumerable<LogRecordModel>> GetAllObjectsAsync(Guid securityTokenId)
    {
        IEnumerable<LogRecordModel> result = default;
        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/Log/GetAllLogRecords");
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Конвертация полученного из WebAPI результата в объект данных.
            result = await JsonSerializer.DeserializeAsync<IEnumerable<LogRecordModel>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        return result;
    }

    /// <summary>
    /// Получение полной информации о записи лога с указанным Id.
    /// </summary>
    public async Task<LogRecordModel> GetObjectDetailsAsync(Guid logRecordId, Guid securityTokenId)
    {
        LogRecordModel result = null;

        // Формирование запроса к WebAPI.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/Log/GetLogRecordById/{logRecordId}");
        // Добавление Токена безопасности в заголовок запроса.
        request.Headers.Add("SecurityTokenBearer", securityTokenId.ToString());
        // Отправка запроса к WebAPI и получение ответа.
        HttpResponseMessage response = await _httpClient.SendAsync(request);

        // Если удалось получить результат от WebAPI, то
        if (response.IsSuccessStatusCode)
        {
            // Конвертация полученного из WebAPI результата в объект данных.
            result = await JsonSerializer.DeserializeAsync<LogRecordModel>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        return result;
    }
}