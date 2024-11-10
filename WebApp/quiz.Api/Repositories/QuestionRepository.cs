using quiz.ModelDb;
using QuestionModel = quiz.ModelBusiness.QuestionModel;

namespace quiz.Api.Repositories;

/// <summary>
/// Репозиторий для управления данными Вопросов в БД.
/// </summary>
public class QuestionRepository : IObjectRepository<QuestionModel>
{
    /// <summary>
    /// Ссылка на EntityFramework-контекст базы данных.
    /// </summary>
    private readonly quizContext _appDbContext;

    public QuestionRepository(quizContext quizContext)
    {
        _appDbContext = quizContext;
    }

    public IEnumerable<QuestionModel> GetAllObjects()
    {
        List<QuestionModel> result = new List<QuestionModel>();

        foreach (Question currentQuestionDbData in _appDbContext.Questions)
        {
            // Вызов функции заполнения модели Вопроса данными из БД.
            result.Add(PopulateQuestionModel(currentQuestionDbData));
        }

        return result;
    }

    public QuestionModel? GetObjectById(Guid id)
    {
        QuestionModel? result = null;
        Question? dbResult = _appDbContext.Questions.FirstOrDefault(i => i.Id == id);

        if (dbResult != null)
        {
            // Вызов функции заполнения модели Вопроса данными из БД.
            result = PopulateQuestionModel(dbResult);
        }

        return result;
    }

    public IEnumerable<QuestionModel> GetObjectsInGroup(Guid groupId)
    {
        List<QuestionModel> result = new List<QuestionModel>();
        List<Question> dbResult = _appDbContext.Questions.ToList();

        foreach (var currentDbRecord in dbResult)
        {
            // Вызов функции заполнения модели Вопроса данными из БД.
            result.Add(PopulateQuestionModel(currentDbRecord));
        }

        return result;
    }

    public QuestionModel AddObject(QuestionModel model)
    {
        Question editingEntity = new Question()
                                   {
                                       Id = model.Id,
                                       TopicId = model.TopicId,
                                       QuestionText = model.QuestionText,
                                       Option01 = model.Option01,
                                       Option02 = model.Option02,
                                       Option03 = model.Option03,
                                       Option04 = model.Option04,
                                       Answer = model.Answer,
                                       CreatedAt = model.CreatedAt
                                   };
        _appDbContext.Questions.Add(editingEntity);
        _appDbContext.SaveChanges();

        return model;
    }

    public QuestionModel UpdateObject(QuestionModel model)
    {
        Question? editingEntity = _appDbContext.Questions.FirstOrDefault(i => i.Id == model.Id);

        if (editingEntity != null)
        {
            editingEntity.Id = model.Id;
            editingEntity.TopicId = model.TopicId;
            editingEntity.QuestionText = model.QuestionText;
            editingEntity.Option01 = model.Option01;
            editingEntity.Option02 = model.Option02;
            editingEntity.Option03 = model.Option03;
            editingEntity.Option04 = model.Option04;
            editingEntity.Answer = model.Answer;
            editingEntity.CreatedAt = model.CreatedAt;

            _appDbContext.SaveChanges();
        }

        return model;
    }

    public void DeleteObject(Guid objectId)
    {
        Question? foundQuestion = _appDbContext.Questions.FirstOrDefault(e => e.Id == objectId);

        if (foundQuestion == null)
        {
            return;
        }

        _appDbContext.Questions.Remove(foundQuestion);
        _appDbContext.SaveChanges();
    }

    /// <summary>
    /// Заполнение модели Вопроса из БД.
    /// </summary>
    private QuestionModel PopulateQuestionModel(
        Question modelDb
    )
    {
        return new QuestionModel(
            modelDb.Id,
            modelDb.TopicId,
            _appDbContext.Topics.SingleOrDefault(i => i.Id == modelDb.TopicId)?.Name ?? string.Empty,
            modelDb.QuestionText,
            modelDb.Option01,
            modelDb.Option02,
            modelDb.Option03,
            modelDb.Option04,
            modelDb.Answer,
            modelDb.CreatedAt
        );
    }
}