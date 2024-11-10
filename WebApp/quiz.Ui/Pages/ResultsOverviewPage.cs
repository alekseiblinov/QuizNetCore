using Microsoft.AspNetCore.Components;

namespace quiz.Ui.Pages;

/// <summary>
/// Страница для прохождения теста и кнопок управления для этого.
/// </summary>
public partial class ResultsOverviewPage
{
    /// <summary>
    /// Id теста.
    /// </summary>
    [Parameter]
    public Guid? QuizId { get; set; }

    /// <summary>
    /// Заголовок страницы редактора.
    /// </summary>
    private string? HeaderText { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        // Установка текста заголовка страницы редактора.
        HeaderText = "Результаты тестирования";
    }
}