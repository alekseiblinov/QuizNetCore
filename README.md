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