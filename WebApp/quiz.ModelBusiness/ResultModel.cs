using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Модель для класса Вопрос.
/// </summary>
public class ResultModel
{
    public string QuestionText { get; set; }
    public string SelectedAnswerText { get; set; }
    public string CorrectAnswerText { get; set; }
    public DateTime AnsweredAt { get; set; }
    public bool IsCorrect { get; set; }
    public int Repetitions { get; set; }
    public int RepetitionInterval { get; set; }
    public DateTime NextDue { get; set; }
    /// <summary>
    /// Удобное для пользователя представление значения "Следующее повторение".
    /// </summary>
    public string NextDueCaption
    {
        get => NextDue.ToShortDateString();

    }

    /// <summary>
    /// Без конструктора по умолчанию объекты не сериализуются в JSON для передачи в WebAPI.
    /// </summary>
#pragma warning disable CS8618
    public ResultModel()
    {
    }
#pragma warning restore CS8618

    public ResultModel(
            string questionText,
            string selectedAnswerText,
            string correctAnswerText,
            DateTime answeredAt,
            bool isCorrect,
            int repetitions,
            int repetitionInterval,
            DateTime nextDue)
    {
        QuestionText = questionText;
        SelectedAnswerText = selectedAnswerText;
        CorrectAnswerText = correctAnswerText;
        AnsweredAt = answeredAt;
        IsCorrect = isCorrect;
        SelectedAnswerText = selectedAnswerText;
        Repetitions = repetitions;
        RepetitionInterval = repetitionInterval;
        NextDue = nextDue;
    }
}