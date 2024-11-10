using quiz.ModelDb;
using TopicModel = quiz.ModelBusiness.TopicModel;

namespace quiz.Api.Repositories;

/// <summary>
/// Репозиторий для управления данными классов Визитов в БД.
/// </summary>
public class TopicRepository : IObjectRepository<TopicModel>
{
    /// <summary>
    /// Ссылка на EntityFramework-контекст базы данных.
    /// </summary>
    private readonly quizContext _appDbContext;

    public TopicRepository(quizContext quizContext)
    {
        _appDbContext = quizContext;
    }

    public IEnumerable<TopicModel> GetAllObjects()
    {
        List<TopicModel> result = new List<TopicModel>();

        foreach (Topic currentTopicDbData in _appDbContext.Topics)
        {
            // Вызов функции заполнения модели класса Визита данными из БД.
            result.Add(PopulateTopicModel(currentTopicDbData));
        }

        return result;
    }

    public TopicModel? GetObjectById(Guid topicId)
    {
        TopicModel? result = null;
        Topic? dbResult = _appDbContext.Topics.FirstOrDefault(i => i.Id == topicId);

        if (dbResult != null)
        {
            // Вызов функции заполнения модели класса Визита данными из БД.
            result = PopulateTopicModel(dbResult);
        }

        return result;
    }

    public IEnumerable<TopicModel> GetObjectsInGroup(Guid groupId)
    {
        List<TopicModel> result = new List<TopicModel>();
        List<Topic> dbResult = _appDbContext.Topics.ToList();

        foreach (var currentDbRecord in dbResult)
        {
            // Вызов функции заполнения модели класса Визита данными из БД.
            result.Add(PopulateTopicModel(currentDbRecord));
        }

        return result;
    }

    public TopicModel AddObject(TopicModel model)
    {
        Topic editingEntity = new Topic()
                                   {
                                       Id = model.Id,
                                       Name = model.Name,
                                       OrderNumber = model.OrderNumber,
                                       CreatedAt = model.CreatedAt
                                   };
        _appDbContext.Topics.Add(editingEntity);
        _appDbContext.SaveChanges();

        return model;
    }

    public TopicModel UpdateObject(TopicModel model)
    {
        Topic? editingEntity = _appDbContext.Topics.FirstOrDefault(i => i.Id == model.Id);

        if (editingEntity != null)
        {
            editingEntity.Id = model.Id;
            editingEntity.Name = model.Name;
            editingEntity.OrderNumber = model.OrderNumber;
            editingEntity.CreatedAt = model.CreatedAt;

            _appDbContext.SaveChanges();
        }

        return model;
    }

    public void DeleteObject(Guid objectId)
    {
        Topic? foundTopic = _appDbContext.Topics.FirstOrDefault(e => e.Id == objectId);

        if (foundTopic == null)
        {
            return;
        }

        _appDbContext.Topics.Remove(foundTopic);
        _appDbContext.SaveChanges();
    }

    /// <summary>
    /// Заполнение модели класса Визита из БД.
    /// </summary>
    private TopicModel PopulateTopicModel(
        Topic modelDb
    )
    {
        return new TopicModel(
            modelDb.Id,
            modelDb.Name,
            modelDb.OrderNumber,
            modelDb.CreatedAt
        );
    }
}