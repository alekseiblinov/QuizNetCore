# QuizNetCore
---------------------------------------------------------------------------
##	The purpose of the project.

This application is a website with several pages and serves to enable simple classical tests with subsequent display of the results for users. The results of the execution are displayed as the number of correct answers and the total number of questions. Information about the correct and incorrect answers of the user is also displayed. To pass the test, the user must pre-register or log in. It is possible to assign the Administrator role to selected users. Users with this role have the ability to manage the accounts of other users, as well as view the test results of any users.
The application also implements the following features:
- The Administrator manages the themes (add, edit and delete) for testing. 
- The Administrator manages the questions (add, edit and delete) for testing. 
- Management of users and roles. 
- Authentication and authorization of users based on simple accounts.
- Recording user actions in the database table (logging).
- A single database data access service to ensure CRUD actions.


##	The technologies used.

- Database management system: Microsoft SQL Server 2-17 (подойдёт и более ранний SQL Server 2-12).
- Platform: .NET 6.-.
- IDE: Microsoft Visual Studio.


##	The history of the appearance.

This project appeared as a result of my study of the Blazor technology in the fall of 2-24 and is a useful exercise. The idea was suggested by my friend Alexander Garbuzov, who asked for help to finalize the training project in Python and MySQL. He kindly allowed us to use the list of test questions and answers. 


##	Well-developed topics.

During the execution, the following topics were studied:
- Components.
- Pages, routing, and layouts.
- Dependency Injection.
- Interaction with data.
- Interaction with Web API.
- Forms and validation.
- Logging.
- Security.
- Deployment.


##	The database structure.

- AspNetRoleClaims - is a part of the security subsystem.
- AspNetRoles - роли. roles. Part of the security subsystem.
- AspNetUserClaims - is a part of the security subsystem.
- AspNetUserLogins - is a part of the security subsystem.
- AspNetUserRoles - assigned roles for each user. Part of the security subsystem.
- AspNetUsers - users. Part of the security subsystem.
- AspNetUserTokens - is a part of the security subsystem.
- LogRecords - user actions in the system (log).
- Question - questions for testing.
- SecurityTokens - used security tokens. Part of the security subsystem.
- Topics - topics for testing.
- UserQuestionProgress - results of user responses to test questions.


##	The folder structure of the project.

- ..\quiz.Api\ - Web API controllers.
o..\quiz.Logger\ - types and functions that ensure the operation of the logging subsystem.
o ..\quiz.ModelBusiness\ - types representing data entities for their subsequent processing and presentation in the UI.
o ..\quiz.ModelDb\ - types representing data entities used when interacting with the database.
- ..\quiz.Shared\ - types and functions shared in Web API and UI methods. This assembly is referenced by the assemblies quiz.Api and quiz.Ui.
o..\quiz.Ui\ - types and functions designed to form the user interface, display it, and enable the user to interact with it.


##	Known issues.

- The number of possible answers to each question is fixed and is equal to four.
- The Administrator does not have the ability to view a list of all tests and select one of them to view the results page. The results page can be opened by knowing the test ID using the URL ~/resultsoverviewpage/<test_id>.
- The absence of the essence of the Quiz. There is no such table in the database, while the UserQuestionProgress test results table has a QuizId field. Such an entity could be useful for storing additional information about each test and for optimizing the database structure, for example, moving the UserId and LastAnswered fields from the UserQuestionProgress table to the Quiz table.
- Suboptimal queries. More data is often selected from database tables than is actually needed. 
  Example: 
  // Getting a list of all the answers to the questions.
  WebApiCallResult<IEnumerable<UserQuestionProgressModel>> webApiCallResultAnswers = UserQuestionProgressesDataServiceInstance.GetAllObjectsAsync(currentUserName).Result;
