using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Класс бизнес-модели, описывающий данные записи Лога.
/// </summary>
public class LogRecordModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Необходимо ввести текст записи лога.")]
    public string Message { get; set; }

    public DateTime? CreatedAt { get; set; }        

    /// <summary>
    /// Без конструктора по умолчанию объекты не сериализуются в JSON для передачи в WebAPI.
    /// </summary>
#pragma warning disable CS8618
    public LogRecordModel()
    {
    }

    public LogRecordModel(Guid recordId, string message, DateTime? createdAt)
    {
        Id = recordId;
        Message = message;
        CreatedAt = createdAt;
    }
}