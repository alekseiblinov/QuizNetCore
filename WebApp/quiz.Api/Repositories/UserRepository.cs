using quiz.ModelBusiness;
using quiz.ModelDb;
using Microsoft.EntityFrameworkCore;
using UserModel = quiz.ModelBusiness.UserModel;

namespace quiz.Api.Repositories;

/// <summary>
/// Репозиторий для управления данными Пользователей в БД.
/// </summary>
public class UserRepository : IObjectRepository<UserModel>
{
    /// <summary>
    /// Ссылка на EntityFramework-контекст базы данных.
    /// </summary>
    private readonly quizContext _appDbContext;

    public UserRepository(quizContext quizContext)
    {
        _appDbContext = quizContext;
    }

    public IEnumerable<UserModel> GetAllObjects()
    {
        List<UserModel> result = new List<UserModel>();
            
        foreach (AspNetUser currentUserDbData in _appDbContext.AspNetUsers.Include(i => i.Roles))
        {
            // Вызов функции заполнения модели Пользователя данными из БД.
            result.Add(PopulateUserModel(currentUserDbData));
        }

        return result;
    }

    public UserModel? GetObjectById(Guid userId)
    {
        UserModel? result = null;
        AspNetUser? dbResult = _appDbContext.AspNetUsers.Include(i => i.Roles).FirstOrDefault(i => i.Id == userId.ToString());

        if (dbResult != null)
        {
            // Вызов функции заполнения модели Пользователя данными из БД.
            result = PopulateUserModel(dbResult);
        }

        return result;
    }

    public IEnumerable<UserModel> GetObjectsInGroup(Guid groupId)
    {
        throw new NotImplementedException("Невозможно получить Пользователей в группе, так как Пользователи являются самым верхним узлом в иерархии.");
    }

    public UserModel AddObject(UserModel model)
    {
        // Не следует добавлять данные Пользователя напрямую в таблицу БД. Для этого требуется использовать UserManager<IdentityUser>.
        throw new NotImplementedException("Не следует добавлять данные пользователя напрямую в таблицу БД.");
    }

    public UserModel UpdateObject(UserModel model)
    {
        // Не следует изменять данные Пользователя напрямую в таблицу БД. Для этого требуется использовать UserManager<IdentityUser>.
        throw new NotImplementedException("Не следует изменять данные пользователя напрямую в таблицу БД.");
    }

    public void DeleteObject(Guid objectId)
    {
        // Не следует удалять данные Пользователя напрямую в таблицу БД. Для этого требуется использовать UserManager<IdentityUser>.
        throw new NotImplementedException("Не следует удалять данные пользователя напрямую в таблицу БД.");
    }

    /// <summary>
    /// Заполнение модели Пользователя данными из БД.
    /// </summary>
    private UserModel PopulateUserModel(
        AspNetUser modelDb
    )
    {
        List<RoleModel> roles = new List<RoleModel>();

        foreach (var currentRole in modelDb.Roles)
        {
            roles.Add(new RoleModel(new Guid(currentRole.Id), currentRole.Name!));
        }

        return new UserModel(
            new Guid(modelDb.Id), 
            modelDb.UserName ?? string.Empty, 
            modelDb.Email ?? string.Empty, 
            string.Empty,
            modelDb.CreatedAt,
            roles
        );
    }
}