namespace quiz.Ui.Pages.Admin;

/// <summary>
/// Страница для просмотра списка Тем и кнопок управления для них.
/// </summary>
public partial class TopicsOverviewPage
{
    /// <summary>
    /// Заголовок страницы редактора.
    /// </summary>
    private string? HeaderText { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        // Установка текста заголовка страницы редактора.
        HeaderText = "Перечень тем для тестирования";
    }
}