using quiz.ModelDb;
using UserQuestionProgressModel = quiz.ModelBusiness.UserQuestionProgressModel;

namespace quiz.Api.Repositories;

/// <summary>
/// Репозиторий для управления данными Ответов на вопросы в БД.
/// </summary>
public class UserQuestionProgressRepository : IObjectRepository<UserQuestionProgressModel>
{
    /// <summary>
    /// Ссылка на EntityFramework-контекст базы данных.
    /// </summary>
    private readonly quizContext _appDbContext;

    public UserQuestionProgressRepository(quizContext quizContext)
    {
        _appDbContext = quizContext;
    }

    public IEnumerable<UserQuestionProgressModel> GetAllObjects()
    {
        List<UserQuestionProgressModel> result = new List<UserQuestionProgressModel>();

        foreach (UserQuestionProgress currentUserQuestionProgressDbData in _appDbContext.UserQuestionProgresses)
        {
            // Вызов функции заполнения модели Ответов на вопрос данными из БД.
            result.Add(PopulateUserQuestionProgressModel(currentUserQuestionProgressDbData));
        }

        return result;
    }

    public UserQuestionProgressModel? GetObjectById(Guid id)
    {
        UserQuestionProgressModel? result = null;
        UserQuestionProgress? dbResult = _appDbContext.UserQuestionProgresses.FirstOrDefault(i => i.Id == id);

        if (dbResult != null)
        {
            // Вызов функции заполнения модели Ответов на вопрос данными из БД.
            result = PopulateUserQuestionProgressModel(dbResult);
        }

        return result;
    }

    public IEnumerable<UserQuestionProgressModel> GetObjectsInGroup(Guid groupId)
    {
        List<UserQuestionProgressModel> result = new List<UserQuestionProgressModel>();
        List<UserQuestionProgress> dbResult = _appDbContext.UserQuestionProgresses.ToList();

        foreach (var currentDbRecord in dbResult)
        {
            // Вызов функции заполнения модели Ответов на вопрос данными из БД.
            result.Add(PopulateUserQuestionProgressModel(currentDbRecord));
        }

        return result;
    }

    public UserQuestionProgressModel AddObject(UserQuestionProgressModel model)
    {
        UserQuestionProgress editingEntity = new UserQuestionProgress()
                                   {
                                       Id = model.Id,
                                       QuizId = model.QuizId,
                                       UserId = model.UserId,
                                       QuestionId = model.QuestionId,
                                       LastAnswered = model.LastAnswered,
                                       IsCorrect = model.IsCorrect,
                                       SelectedAnswerText = model.SelectedAnswerText,
                                       Repetitions = model.Repetitions,
                                       RepetitionInterval = model.RepetitionInterval,
                                       NextDue = model.NextDue
                                   };
        _appDbContext.UserQuestionProgresses.Add(editingEntity);
        _appDbContext.SaveChanges();

        return model;
    }

    public UserQuestionProgressModel UpdateObject(UserQuestionProgressModel model)
    {
        UserQuestionProgress? editingEntity = _appDbContext.UserQuestionProgresses.FirstOrDefault(i => i.Id == model.Id);

        if (editingEntity != null)
        {
            editingEntity.Id = model.Id;
            editingEntity.QuizId = model.QuizId;
            editingEntity.UserId = model.UserId;
            editingEntity.QuestionId = model.QuestionId;
            editingEntity.LastAnswered = model.LastAnswered;
            editingEntity.IsCorrect = model.IsCorrect;
            editingEntity.SelectedAnswerText = model.SelectedAnswerText;
            editingEntity.Repetitions = model.Repetitions;
            editingEntity.RepetitionInterval = model.RepetitionInterval;
            editingEntity.NextDue = model.NextDue;

            _appDbContext.SaveChanges();
        }

        return model;
    }

    public void DeleteObject(Guid objectId)
    {
        UserQuestionProgress? foundUserQuestionProgress = _appDbContext.UserQuestionProgresses.FirstOrDefault(e => e.Id == objectId);

        if (foundUserQuestionProgress == null)
        {
            return;
        }

        _appDbContext.UserQuestionProgresses.Remove(foundUserQuestionProgress);
        _appDbContext.SaveChanges();
    }

    /// <summary>
    /// Заполнение модели Ответов на вопрос из БД.
    /// </summary>
    private UserQuestionProgressModel PopulateUserQuestionProgressModel(
        UserQuestionProgress modelDb
    )
    {
        return new UserQuestionProgressModel(
            modelDb.Id,
            modelDb.QuizId, 
            modelDb.UserId, 
            modelDb.QuestionId,
            modelDb.LastAnswered,
            modelDb.IsCorrect,
            modelDb.SelectedAnswerText,
            modelDb.Repetitions,
            modelDb.RepetitionInterval,
            modelDb.NextDue
        );
    }
}