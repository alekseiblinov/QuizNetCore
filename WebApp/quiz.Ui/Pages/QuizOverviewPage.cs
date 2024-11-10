using Microsoft.AspNetCore.Components;

namespace quiz.Ui.Pages;

/// <summary>
/// Страница для прохождения теста и кнопок управления для этого.
/// </summary>
public partial class QuizOverviewPage
{
    /// <summary>
    /// Id темы теста.
    /// </summary>
    [Parameter]
    public Guid? TopicId { get; set; }

    /// <summary>
    /// Заголовок страницы редактора.
    /// </summary>
    private string? HeaderText { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        // Установка текста заголовка страницы редактора.
        HeaderText = "Тестирование";
    }
}