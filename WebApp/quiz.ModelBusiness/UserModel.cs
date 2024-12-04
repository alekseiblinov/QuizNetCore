using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Данные пользователя.
/// </summary>
public class UserModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Email { get; set; }
 
    //[Required] Этот атрибут установить нельзя, потому что при редактировании значение пароля пользователя в модели заполняется пробелами (трюк). За проверку корректности значения пароля отвечает атрибут [MinLength].
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Длина пароля должна быть не менее 6 символов.")]
    public string Password { get; set; }

    public List<RoleModel> Roles { get; set; }

    /// <summary>
    /// Текстовое представление списка ролей пользователя для отображения в столбце таблицы.
    /// </summary>
    public string RolesCaption
    {
        get
        {
            return GetUsersRolesStringById(Roles);
        }
    }

    public DateTime CreatedAt { get; set; }

    public UserModel(Guid id, string login, string email, string password, DateTime createdAt
            , List<RoleModel> roles
        )
    {
        Id = id;
        Login = login;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
        Roles = roles;
    }

    /// <summary>
    /// Получение в виде единой строки списка ролей пользователя по его Id.
    /// </summary>
    private string GetUsersRolesStringById(List<RoleModel>? roles)
    {
        string result = string.Empty;

        if (roles != null)
        {
            // Перебор ролей пользователя с указанным Id и составление строки с перечислением его ролей.
            foreach (var currentUserRole in roles)
            {
                result = !string.IsNullOrWhiteSpace(result)
                    ? $"{result}; {currentUserRole.Name}"
                    : currentUserRole.Name;
            }
        }

        return result;
    }
}