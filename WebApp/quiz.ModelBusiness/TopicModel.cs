using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Модель для класса Тема.
/// </summary>
public class TopicModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = $"Необходимо ввести {nameof(Name)}.")]
    [StringLength(255, ErrorMessage = $"{nameof(Name)} не должен быть длиннее 255 символов.")]
    [MinLength(1, ErrorMessage=$"{nameof(Name)} не должен быть короче 1 символа.")]
    public string Name { get; set; }

    [Required(ErrorMessage = $"Необходимо ввести {nameof(OrderNumber)}.")]
    public int OrderNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Без конструктора по умолчанию объекты не сериализуются в JSON для передачи в WebAPI.
    /// </summary>
#pragma warning disable CS8618
    public TopicModel()
    {
    }
#pragma warning restore CS8618

    public TopicModel(Guid id, string name, int orderNumber, DateTime createdAt)
    {
        Id = id;
        Name = name;
        OrderNumber = orderNumber;
        CreatedAt = createdAt;
    }
}