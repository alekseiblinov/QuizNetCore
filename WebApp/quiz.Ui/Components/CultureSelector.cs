using System.Globalization;

namespace quiz.Ui.Components;

/// <summary>
/// Комбобокс со списком языков для переключения языка локализации компонентов UI.
/// </summary>
public partial class CultureSelector
{
    CultureInfo Culture 
    {
        get => CultureInfo.CurrentCulture;
        set 
        {
            // Если пользователь выбрал в выпадающем списке комбобокса язык, отличный от прежнего, то
            if (!Equals(CultureInfo.CurrentCulture, value)) 
            {
                // Формирование запроса к WebAPI.
                string uri = new Uri(Nav.Uri).GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped);
                string cultureEscaped = Uri.EscapeDataString(value.Name);
                string uriEscaped = Uri.EscapeDataString(uri);

                // Выполнение запроса к WebAPI и обращение к контроллеру. Контроллер добавит cookie с указанием Culture языка и выполнит redirect на прежнюю страницу UI. HARDCODE. Привязка к backend web API URL https://localhost:7115/.
                Nav.NavigateTo(
                    $"https://localhost:7115/Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}",
                    forceLoad: true
                );

                // Другой вариант. Здесь можно передать готовый объект HttpClient с настроенным URL для обращения чтобы избежать hardcode-привязки к backend web API URL.
                //// Формирование запроса к WebAPI.
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7115/Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}");
                //// Отправка запроса к WebAPI и получение ответа.
                //HttpResponseMessage response = _httpClient.Send(request);
            }
        }
    }
}