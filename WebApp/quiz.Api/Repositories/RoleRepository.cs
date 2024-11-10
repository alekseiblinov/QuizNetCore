using quiz.ModelDb;
using RoleModel = quiz.ModelBusiness.RoleModel;

namespace quiz.Api.Repositories;

/// <summary>
/// Репозиторий для управления данными Ролей в БД.
/// </summary>
public class RoleRepository : IObjectRepository<RoleModel>
{
    /// <summary>
    /// Ссылка на EntityFramework-контекст базы данных.
    /// </summary>
    private readonly quizContext _appDbContext;

    public RoleRepository(quizContext quizContext)
    {
        _appDbContext = quizContext;
    }

    public IEnumerable<RoleModel> GetAllObjects()
    {
        List<RoleModel> result = new List<RoleModel>();
            
        foreach (AspNetRole currentRoleDbData in _appDbContext.AspNetRoles)
        {
            // Вызов функции заполнения модели Роли данными из БД.
            result.Add(PopulateRoleModel(currentRoleDbData));
        }

        return result;
    }

    public RoleModel? GetObjectById(Guid roleId)
    {
        RoleModel? result = null;
        AspNetRole? dbResult = _appDbContext.AspNetRoles.FirstOrDefault(i => i.Id == roleId.ToString());

        if (dbResult != null)
        {
            // Вызов функции заполнения модели Роли данными из БД.
            result = PopulateRoleModel(dbResult);
        }

        return result;
    }

    public IEnumerable<RoleModel> GetObjectsInGroup(Guid groupId)
    {
        throw new NotImplementedException("Невозможно получить Ролей в группе, так как Роли являются самым верхним узлом в иерархии.");
    }

    public RoleModel AddObject(RoleModel model)
    {
        // Не следует добавлять данные Роли напрямую в таблицу БД. Для этого требуется использовать RoleManager<IdentityRole>.
        throw new NotImplementedException("Не следует добавлять данные роли напрямую в таблицу БД.");
    }

    public RoleModel UpdateObject(RoleModel model)
    {
        // Не следует изменять данные Роли напрямую в таблицу БД. Для этого требуется использовать RoleManager<IdentityRole>.
        throw new NotImplementedException("Не следует изменять данные роли напрямую в таблицу БД.");
    }

    public void DeleteObject(Guid objectId)
    {
        // Не следует удалять данные Роли напрямую в таблицу БД. Для этого требуется использовать RoleManager<IdentityRole>.
        throw new NotImplementedException("Не следует удалять данные роли напрямую в таблицу БД.");
    }

    /// <summary>
    /// Заполнение модели Роли данными из БД.
    /// </summary>
    private RoleModel PopulateRoleModel(
        AspNetRole modelDb
    )
    {
        return new RoleModel(
            new Guid(modelDb.Id), 
            modelDb.Name ?? string.Empty
        );
    }
}