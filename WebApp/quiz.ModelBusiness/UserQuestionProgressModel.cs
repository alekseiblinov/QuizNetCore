using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Модель для Ответов пользователя на тест.
/// </summary>
public class UserQuestionProgressModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = $"Необходимо ввести {nameof(UserId)}.")]
    public string UserId { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(QuizId)}.")]
    public Guid QuizId { get; set; }

    [Required(ErrorMessage = $"Необходимо ввести {nameof(QuestionId)}.")]
    public Guid QuestionId { get; set; }

    [Required(ErrorMessage = $"Необходимо ввести {nameof(LastAnswered)}.")]
    public DateTime LastAnswered { get; set; }

    public bool IsCorrect { get; set; }

    public string SelectedAnswerText { get; set; }

    public int Repetitions { get; set; }

    public int RepetitionInterval { get; set; }

    public DateTime NextDue { get; set; }

    /// <summary>
    /// Без конструктора по умолчанию объекты не сериализуются в JSON для передачи в WebAPI.
    /// </summary>
#pragma warning disable CS8618
    public UserQuestionProgressModel()
    {
    }
#pragma warning restore CS8618

    public UserQuestionProgressModel(
        Guid id,
        Guid quizId,
        string userId,
        Guid questionId,
        DateTime lastAnswered,
        bool isCorrect,
        string selectedAnswerText,
        int repetitions,
        int repetitionInterval,
        DateTime nextDue
        )
    {
        Id = id;
        QuizId = quizId;
        UserId = userId;
        QuestionId = questionId;
        LastAnswered = lastAnswered;
        IsCorrect = isCorrect;
        SelectedAnswerText = selectedAnswerText;
        Repetitions = repetitions;
        RepetitionInterval = repetitionInterval;
        NextDue = nextDue;
    }
}