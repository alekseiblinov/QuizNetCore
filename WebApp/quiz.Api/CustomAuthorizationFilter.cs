namespace quiz.Api;

using Microsoft.AspNetCore.Mvc.Controllers;
using Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Фильтр, проверяющий корректность токена при каждом вызове метода контроллера. Данный атрибут может быть применён к определённому методу контроллера или ко всему контроллеру целиком.
/// По материалам https://stackoverflow.com/questions/29915192/dependency-injection-in-attributes.
/// </summary>
public class SecurityTokenValidationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            // Получение объекта репозитория для управления аутентификацией и авторизацией пользователей. Сделать это с помощью DI в атрибутаж не представляется возможным (см. https://stackoverflow.com/questions/29915192/dependency-injection-in-attributes).
            ISecurityRepository? securityRepository = context.HttpContext.RequestServices.GetService(typeof(ISecurityRepository)) as ISecurityRepository;
            // Получение значения Токена безопасности из заголовка запроса.
            context.HttpContext.Request.Headers.TryGetValue("SecurityTokenBearer", out StringValues securityTokenId);
            // Получение из запроса объекта с данными о вызывающем это метод коде.
            ControllerActionDescriptor? actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            // Если удалось получить все необходимые данные, то 
            if (securityRepository != null && actionDescriptor != null)
            {
                // Если Токен безопасности не прошел проверку, то
                if (!securityRepository.IsSecurityTokenValid(securityTokenId, context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress?.ToString(), callerMethodFullName: $"{actionDescriptor.ControllerName}_{actionDescriptor.ActionName}"))
                {
                    // Возвращение соответствующей ошибки.
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
            }
            else
            {
                // В случае отсутствия необходимых для проверки Токена данных возвращение состояния ошибки.
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        catch (Exception)
        {
            // В случае возникновения какой-либо ошибки в ходе проверки Токена возвращение состояния ошибки.
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
