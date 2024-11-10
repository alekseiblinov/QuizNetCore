using quiz.Api;
using quiz.Api.Repositories;
using quiz.Logger;
using quiz.Logger.Repositories;
using quiz.ModelBusiness;
using quiz.ModelDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Получение объекта управления настройками приложения.
ConfigurationManager configuration = builder.Configuration;

// Добавление в контейнер зависимостей объекта основного контекста БД. Он должен создаваться только на момент использования, чтобы не происходило замеченных ошибок одновременного доступа из разных потоков при логгировании сообщений.
builder.Services.AddDbContext<quizContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), 
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);
// Добавление в контейнер зависимостей объекта контекста БД для управления Identity.
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);

// Добавление сервиса для обслуживания Identity. По материалам https://metanit.com/sharp/aspnet5/16.11.php.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                                                         {
                                                             options.SignIn.RequireConfirmedAccount = false;     // #bam_подтверждение_по_email. Отключена необходимость подтверждения нового пользователя по электронной почте.
                                                             // Настройка требований к сложности пароля. По материалам https://metanit.com/sharp/aspnet5/16.9.php.
                                                             options.Password.RequiredLength = 5;               // Минимальная длина.
                                                             options.Password.RequireNonAlphanumeric = false;   // Требуются ли не алфавитно-цифровые символы.
                                                             options.Password.RequireLowercase = false;         // Требуются ли символы в нижнем регистре.
                                                             options.Password.RequireUppercase = false;         // Требуются ли символы в верхнем регистре.
                                                             options.Password.RequireDigit = false;             // Требуются ли цифры.
                                                             // Настройка требований к логину пользователя. По материалам https://metanit.com/sharp/aspnet5/16.10.php.
                                                             options.User.RequireUniqueEmail = true;                                    // Требуются ли уникальность email.
                                                             options.User.AllowedUserNameCharacters = "._ @1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";   // Допустимые символы.
                                                         }) 
    .AddEntityFrameworkStores<IdentityDbContext>();

// Добавление сервисов для работы с данными в БД.
builder.Services.AddScoped<IObjectRepository<TopicModel>, TopicRepository>();
builder.Services.AddScoped<IObjectRepository<QuestionModel>, QuestionRepository>();
builder.Services.AddScoped<IObjectRepository<UserQuestionProgressModel>, UserQuestionProgressRepository>();
builder.Services.AddScoped<IObjectRepository<UserModel>, UserRepository>();
builder.Services.AddScoped<IObjectRepository<RoleModel>, RoleRepository>();
// Добавление в контейнер зависимостей сервиса для управления записями лога и его репозитория. Они должны создаваться только на момент использования для предотвращения ошибки "В этом контексте была начата вторая операция до завершения предыдущей операции. Как правило, так происходит, когда несколько потоков используют один и тот же экземпляр DbContext. ...", описанной в https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/#avoiding-dbcontext-threading-issues.
builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<ILogDbDirect, LogServiceDbDirect>();
builder.Services.AddTransient<ISecurityRepository, SecurityRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
