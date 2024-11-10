using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Данные роли пользователя.
/// </summary>
public class RoleModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public RoleModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}