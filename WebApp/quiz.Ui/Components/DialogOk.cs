using Microsoft.AspNetCore.Components;

namespace quiz.Ui.Components;

/// <summary>
/// Popup-диалог с надписью и кнопкой "OK".
/// </summary>
/// #bam_диалог_Ок.
public partial class DialogOk
{
    /// <summary>
    /// Отображается ли данный диалог на экране пользователю.
    /// </summary>
    private bool ShowDialog { get; set; }

    /// <summary>
    /// Событие, которое возникает после закрытия этого popup-диалога.
    /// </summary>
    [Parameter]
    public EventCallback DialogClosedEventCallback { get; set; }
        
    /// <summary>
    /// Текст заголовка, выводимого в окне.
    /// </summary>
    [Parameter]
    public string? HeaderText { get; set; }

    /// <summary>
    /// Текст сообщения, выводимого в окне (надпись).
    /// </summary>
    [Parameter]
    public string? MessageText { get; set; }

    /// <summary>
    /// Отобразить диалог на экране пользователю.
    /// </summary>
    public void Show()
    {
        // Отображение диалога на экран пользователю.
        ShowDialog = true;
        // Перерисовка содержимого веб-страницы для пользователя.
        StateHasChanged();
    }

    /// <summary>
    /// Обработка нажатия пользователем кнопки "OK".
    /// </summary>
    private async void OkAsync()
    {
        // Закрытие диалогового окна.
        ShowDialog = false;
        // Вызов события, обозначающего закрытие этого диалогового окна.
        await DialogClosedEventCallback.InvokeAsync(null);
        // Перерисовка содержимого веб-страницы для пользователя.
        StateHasChanged();
    }
}