using System.Web;
using Microsoft.AspNetCore.Components;

namespace quiz.Ui.Pages.Admin;

/// <summary>
/// Страница для просмотра списка Вопросов и кнопок управления для них.
/// </summary>
public partial class QuestionsOverviewPage
{
    /// <summary>
    /// Заголовок страницы редактора.
    /// </summary>
    private string? HeaderText { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        // Установка текста заголовка страницы редактора.
        HeaderText = "Перечень вопросов для тестирования";
    }
}