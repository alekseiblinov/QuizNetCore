using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace quiz.Api.Controllers 
{
    /// <summary>
    /// Контроллер для управления переадресацией после изменения языка UI локализации приложения.
    /// По материалам из примера https://github.com/DevExpress-Examples/localize-devexpress-blazor-components.
    /// </summary>
    [Route("[controller]/[action]")]
    public class CultureController : Controller 
    {
        public IActionResult Set(string? culture, string redirectUri) 
        {
            if (culture != null) 
            {
                // В ответ добавляется cookie с указанием Culture для языка, который устанавливается с использованием переданных параметров.
                HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(culture, culture)));
            }
            
            return Redirect(redirectUri);
        }
    }
}