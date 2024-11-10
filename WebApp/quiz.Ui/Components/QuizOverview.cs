using quiz.ModelBusiness;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Web;

namespace quiz.Ui.Components;

/// <summary>
/// Страница проведения опроса. На ней отображается список вопросов и вариантов ответов.
/// </summary>
public partial class QuizOverview
{
    /// <summary>
    /// Количество вопросов в тесте.
    /// </summary>
    private const int QUESTIONS_PER_QUIZ_COUNT = 5;

    // Отключение предупреждения "not-null property is uninitialized" для инжектируемых объектов и сервисов.
#pragma warning disable CS8618
    /// <summary>
    /// Ссылка на сервис для выполнения перенаправления на адреса Url. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManagerInstance { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных объектов. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private IObjectDataService<QuestionModel> QuestionsDataServiceInstance { get; set; }

    /// <summary>
    /// Ссылка на сервис для передачи в БД и получения из нее данных об ответах на тесты. Внедряется с помощью DI.
    /// </summary>
    [Inject]
    private IObjectDataService<UserQuestionProgressModel> UserQuestionProgressesDataServiceInstance { get; set; }

    /// <summary>
    /// Объект с информацией о текущем залогиненном пользователе.
    /// </summary>
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Id темы теста.
    /// </summary>
    [Parameter]
    public Guid? TopicId { get; set; }

    /// <summary>
    /// Перечень выбранных для теста вопросов.
    /// </summary>
    private List<QuestionModel> QuestionsForQuiz { get; set; } = new List<QuestionModel>();

    /// <summary>
    /// Вносил ли пользователь изменения в данные на странице.
    /// </summary>
    private bool HasChanges { get; set; }

    /// <summary>
    /// Присутствуют ли ошибки в редактируемых данных.
    /// </summary>
    private bool HasErrors { get; set; }

    /// <summary>
    /// Текст сообщения о результате работы редактора.
    /// </summary>
    private string? ResultDescription { get; set; }

    /// <summary>
    /// Загружены ли данные в контрол полностью, или ещё производится загрузка.
    /// </summary>
    private bool IsLoaded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Получение имени залогиненного пользователя.
        string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
        // Заполнение перечня вопросов.
        WebApiCallResult<IEnumerable<QuestionModel>> webApiCallResultQuestions = await QuestionsDataServiceInstance.GetAllObjectsAsync(currentUserName);

        // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
        if (webApiCallResultQuestions.HasErrors)
        {
            // Вывод пользователю информации об этом.
            HasErrors = webApiCallResultQuestions.HasErrors;
            ResultDescription = webApiCallResultQuestions.ResultDescription;
            // Дальнейшее выполнение функции прекращается.
            return;
        }

        // Если данные через WebAPI успешно получены, то 
        if (webApiCallResultQuestions.Data != null)
        {
            // Получение перечня всех Ответов на вопросы для залогиненного пользователя.
            WebApiCallResult<IEnumerable<UserQuestionProgressModel>> webApiCallResultAnswers = await UserQuestionProgressesDataServiceInstance.GetAllObjectsAsync(currentUserName);

            // Если в процессе получения данных от WebAPI были зарегистрированы ошибки, то
            if (webApiCallResultAnswers.HasErrors)
            {
                // Вывод пользователю информации об этом.
                HasErrors = webApiCallResultAnswers.HasErrors;
                ResultDescription = webApiCallResultAnswers.ResultDescription;
                // Дальнейшее выполнение функции прекращается.
                return;
            }

            // Если данные через WebAPI успешно получены, то 
            if (webApiCallResultAnswers.Data != null)
            {
                // Получение из БД коллекции всех ответов текущего пользователя.
                List<UserQuestionProgressModel> shuffledCurrentUserAnswers = webApiCallResultAnswers.Data.Where(i => i.UserId == currentUserName).ToList();
                // Перемешивание полученной из БД коллекции ответов текущего пользователя.
                shuffledCurrentUserAnswers.Shuffle();

                // Перебор всех ответов, для которых пришёл срок повторения.
                foreach (UserQuestionProgressModel currentAnswer in shuffledCurrentUserAnswers.Where(i => i.NextDue <= DateTime.Now).DistinctBy(i => i.QuestionId))
                {
                    // Получение данных вопроса для рассматриваемого ответа. Также учитывается то, что вопрос должен относиться к определённой теме.
                    QuestionModel? dueQuestion = webApiCallResultQuestions.Data.Where(i => i.TopicId == TopicId).FirstOrDefault(i => i.Id == currentAnswer.QuestionId );
                    
                    // Если удалось получить данные вопроса для рассматриваемого ответа, то
                    if (dueQuestion != null)
                    {
                        // Добавление информации о вопросе в формируемую коллекцию вопросов для теста.
                        QuestionsForQuiz.Add(dueQuestion);

                        // Количество выбранных для теста вопросов не должно превышать определённого числа.
                        if (QuestionsForQuiz.Count >= QUESTIONS_PER_QUIZ_COUNT)
                        {
                            break;
                        }
                    }
                }

                // Если количество вопросов, которые пора повторит, недостаточно для проведения теста, то
                if (QuestionsForQuiz.Count < QUESTIONS_PER_QUIZ_COUNT)
                {
                    // Выбор из БД вопросов, относящихся к определённой теме. Из коллекции следует исключить вопросы, выбранные в неё ранее.
                    List<QuestionModel> notDueQuestions = webApiCallResultQuestions.Data.Where(i => i.TopicId == TopicId).Except(QuestionsForQuiz).ToList();
                    // Выбор случайным образом и добавление недостающего количества вопросов из полной коллекции вопросов.
                    QuestionsForQuiz.AddRange(notDueQuestions.Take(QUESTIONS_PER_QUIZ_COUNT - QuestionsForQuiz.Count));
                }

                // Перемешивание полученной из БД коллекции вопросов.
                QuestionsForQuiz.Shuffle();

                // Сброс признака, обозначающего наличие ошибок при взаимодействии с WebAPI.
                HasErrors = false;
            }
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        // Установка признака, обозначающего что данные в контрол загружены полностью.
        IsLoaded = true;
    }
    
    /// <summary>
    /// Обработка нажатия на кнопку "К выбору темы".
    /// </summary>
    private async void NavigateToTopicChoosePage(MouseEventArgs arg)
    {
        // Переход к странице выбора темы.
        NavigationManagerInstance.NavigateTo("TopicChooseOverviewPage");
    }

    /// <summary>
    /// Обработка нажатия на кнопку "Сохранить".
    /// </summary>
    private async void OnSubmitClick(MouseEventArgs obj)
    {
        // Генерация Id текущего теста для сохранения.
        Guid currentQuizId = Guid.NewGuid();
        // Получение имени залогиненного пользователя.
        string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;

        // Перебор данных выбранных для теста вопросов.
        foreach (var proceedingQuestion in QuestionsForQuiz)
        {
            // Получение текста выбранного пользователем ответа.
            string selectedAnswerText = quiz.Shared.CommonLogic.SelectedAnswerTextGet(proceedingQuestion);
            // Вычисление является ли данный пользователем ответ корректным.
            bool isCorrect = !string.IsNullOrEmpty(selectedAnswerText) && proceedingQuestion.Answer == selectedAnswerText;
            // Вычисление количества дней до следующего появления вопроса.
            int repetitionIntervalDays = isCorrect 
                ? 7 
                : 0;
            // Вычисление даты и времени следующего появления вопроса.
            DateTime nextDue = DateTime.Now.AddDays(repetitionIntervalDays);

            // Создание нового объекта с данными ответов.
            UserQuestionProgressModel newUserQuestionProgressObject = new UserQuestionProgressModel
                                                                      {
                                                                          Id = Guid.NewGuid(),
                                                                          QuizId = currentQuizId, 
                                                                          UserId = (await AuthenticationStateTask).User.Identity?.Name ?? string.Empty, 
                                                                          QuestionId = proceedingQuestion.Id,
                                                                          LastAnswered = DateTime.Now,
                                                                          IsCorrect = isCorrect,
                                                                          SelectedAnswerText = selectedAnswerText,
                                                                          Repetitions = 0,
                                                                          RepetitionInterval = repetitionIntervalDays,
                                                                          NextDue = nextDue
                                                                      };
            // Вызов функции добавления данных Ответа в БД.
            WebApiCallResult<UserQuestionProgressModel> webApiCallResult = await UserQuestionProgressesDataServiceInstance.AddObjectAsync(newUserQuestionProgressObject, currentUserName);

            // Если в процессе взаимодействия с WebAPI были зарегистрированы ошибки, то
            if (webApiCallResult.HasErrors)
            {
                // Вывод пользователю информации об этом.
                HasErrors = webApiCallResult.HasErrors;
                ResultDescription = webApiCallResult.ResultDescription;
                // Дальнейшее выполнение функции прекращается.
                return;
            }
        }

        // Если ошибок при сохранении не обнаружено, то
        if (!HasErrors)
        {
            // Формирование ссылки на страницу результатов.
            string resultPageUrl = $"/resultsoverviewpage/{currentQuizId}";
            // Переход (возврат) к странице результатов.
            NavigationManagerInstance.NavigateTo(resultPageUrl);
        }
    }
}