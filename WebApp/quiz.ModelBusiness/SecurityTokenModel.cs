using System.ComponentModel.DataAnnotations;

namespace quiz.ModelBusiness;

/// <summary>
/// Класс бизнес-модели, описывающий данные пользователя для входа.
/// </summary>
public class SecurityTokenModel
{
    [Key]
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string MethodName { get; set; }

    public string MethodParametersHash { get; set; }

    public SecurityTokenModel(Guid id, string userName, string methodName, string methodParametersHash)
    {
        Id = id;
        UserName = userName;
        MethodName = methodName;
        MethodParametersHash = methodParametersHash;
    }
}