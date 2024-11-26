using quiz.ModelBusiness;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace quiz.Ui.Components;

/// <summary>
/// Страница с результатами тестов.
/// </summary>
public partial class ResultsOverview
{
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
    /// Id теста.
    /// </summary>
    [Parameter]
    public Guid? QuizId { get; set; }

    /// <summary>
    /// Перечень данных Ответов на вопросы для теста с указанным Id.
    /// </summary>
    private List<UserQuestionProgressModel> AnswersInQuiz { get; set; } = new List<UserQuestionProgressModel>();

    /// <summary>
    /// Список вопросов, на которые был дан корректный ответ.
    /// </summary>
    private List<ResultModel> CorrectAnswers { get; set; } = new List<ResultModel>();

    /// <summary>
    /// Список вопросов, на которые был дан некорректный ответ.
    /// </summary>
    private List<ResultModel> IncorrectAnswers { get; set; } = new List<ResultModel>();

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

    /// <summary>
    /// Разрешено ли пользователю просматривать результаты теста.
    /// </summary>
    private bool CanUserSeeQuizResults
    {
        get
        {
            bool result = false;

            // Пользователю разрешено просматривать результаты теста если он принадлежит к роли Администратора или если он является тем пользователем, который проходил тест. 
            // Это результат желания реализовать концепцию Policy-Based Access Control (PBAC). Более простой и не менее эффективный способ.
            // Если пользователь входит в роль Администраторов, то
            if (AuthenticationStateTask.Result.User.IsInRole("Администраторы"))  // Hardcode! Привязка к имени роли.
            {
                result = true;
            }
            else
            {
                // Получение имени залогиненного пользователя.
                string? currentUserName = AuthenticationStateTask.Result.User.Identity?.Name;
                // Получение имени пользователя, который проходил тест.
                UserQuestionProgressModel oneAnswerInQuiz = AnswersInQuiz.First(i => i.QuizId == QuizId);

                // Если текущий пользователь является тем пользователем, который проходил тест, то
                if (!string.IsNullOrWhiteSpace(oneAnswerInQuiz.UserId) 
                    && oneAnswerInQuiz.UserId == currentUserName)
                {
                    result = true;
                }
            }

            return result;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        // Получение имени залогиненного пользователя.
        string? currentUserName = (await AuthenticationStateTask).User.Identity?.Name;
        // Получение перечня всех Ответов на вопросы.
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
            // Заполнение перечня Ответов на вопросы для теста с указанным Id.
            AnswersInQuiz = webApiCallResultAnswers.Data.Where(i => i.QuizId == QuizId).ToList();
            // Получение перечня участвовавших в тесте Вопросов.
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
                foreach (UserQuestionProgressModel currentAnswer in AnswersInQuiz)
                {
                    // Заполнение перечня участвовавших в тесте Вопросов.
                    foreach (QuestionModel currentQuestion in webApiCallResultQuestions.Data)
                    {
                        if (currentQuestion.Id == currentAnswer.QuestionId)
                        {
                            if (currentAnswer.IsCorrect)
                            {
                                CorrectAnswers.Add(new ResultModel(
                                    currentQuestion.QuestionText,
                                    currentQuestion.Answer,
                                    currentQuestion.Answer,
                                    currentQuestion.CreatedAt,
                                    currentAnswer.IsCorrect,
                                    currentAnswer.Repetitions,
                                    currentAnswer.RepetitionInterval,
                                    currentAnswer.NextDue
                                    ));
                            }
                            else
                            {
                                IncorrectAnswers.Add(new ResultModel(
                                    currentQuestion.QuestionText,
                                    currentAnswer.SelectedAnswerText,
                                    currentQuestion.Answer,
                                    currentQuestion.CreatedAt,
                                    currentAnswer.IsCorrect,
                                    currentAnswer.Repetitions,
                                    currentAnswer.RepetitionInterval,
                                    currentAnswer.NextDue
                                ));
                            }

                            break;
                        }
                    }
                }

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
}