namespace quiz.Ui.Pages;

/// <summary>
/// Страница для выбора темы для прохождения теста.
/// </summary>
public partial class TopicChooseOverviewPage
{
    /// <summary>
    /// Заголовок страницы редактора.
    /// </summary>
    private string? HeaderText { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        // Установка текста заголовка страницы редактора.
        HeaderText = "Выберете тему для тестирования";
    }
}