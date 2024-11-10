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

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Класс HTML для отображения строки с данными Пользователя.
    /// </summary>
    public string UserHtmlClass
    {
        get
        {
            // Текст строк у пользователей по определённому признаку можно сделать выделенным.
            return "align-middle";
        }
    }

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
}