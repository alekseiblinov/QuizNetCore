using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Модель для класса Вопрос.
/// </summary>
public class QuestionModel
{
    [Key]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(TopicId)}.")]
    public Guid TopicId { get; set; }
    
    public string TopicName { get; set; }

    [Required(ErrorMessage = $"Необходимо ввести {nameof(QuestionText)}.")]
    [StringLength(500, ErrorMessage = $"{nameof(QuestionText)} не должен быть длиннее 500 символов.")]
    [MinLength(5, ErrorMessage=$"{nameof(QuestionText)} не должен быть короче 5 символов.")]
    public string QuestionText { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(Option01)}.")]
    [StringLength(255, ErrorMessage = $"{nameof(Option01)} не должен быть длиннее 255 символов.")]
    [MinLength(1, ErrorMessage=$"{nameof(Option01)} не должен быть короче 1 символа.")]
    public string Option01 { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(Option02)}.")]
    [StringLength(255, ErrorMessage = $"{nameof(Option02)} не должен быть длиннее 255 символов.")]
    [MinLength(1, ErrorMessage=$"{nameof(Option02)} не должен быть короче 1 символа.")]
    public string? Option02 { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(Option03)}.")]
    [StringLength(255, ErrorMessage = $"{nameof(Option03)} не должен быть длиннее 255 символов.")]
    [MinLength(1, ErrorMessage=$"{nameof(Option03)} не должен быть короче 1 символа.")]
    public string? Option03 { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(Option04)}.")]
    [StringLength(255, ErrorMessage = $"{nameof(Option04)} не должен быть длиннее 255 символов.")]
    [MinLength(1, ErrorMessage=$"{nameof(Option04)} не должен быть короче 1 символа.")]
    public string? Option04 { get; set; }

    public bool OptionIsSelected01 { get; set; }
    public bool OptionIsSelected02 { get; set; }
    public bool OptionIsSelected03 { get; set; }
    public bool OptionIsSelected04 { get; set; }
    
    [Required(ErrorMessage = $"Необходимо ввести {nameof(Answer)}.")]
    [StringLength(255, ErrorMessage = $"{nameof(Answer)} не должен быть длиннее 255 символов.")]
    [MinLength(1, ErrorMessage=$"{nameof(Answer)} не должен быть короче1 символа.")]
    public string Answer { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Без конструктора по умолчанию объекты не сериализуются в JSON для передачи в WebAPI.
    /// </summary>
#pragma warning disable CS8618
    public QuestionModel()
    {
    }
#pragma warning restore CS8618

    public QuestionModel(
        Guid id, 
        Guid topicId,
        string topicName,
        string questionText,
        string option01,
        string? option02,
        string? option03,
        string? option04,
        string answer,
        DateTime createdAt)
    {
        Id = id;
        TopicId = topicId;
        TopicName = topicName;
        QuestionText = questionText;
        Option01 = option01;
        Option02 = option02;
        Option03 = option03;
        Option04 = option04;
        Answer = answer;
        CreatedAt = createdAt;
    }
}