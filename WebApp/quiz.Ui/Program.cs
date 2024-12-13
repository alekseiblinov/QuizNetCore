using quiz.Logger;
using quiz.Logger.Repositories;
using quiz.ModelBusiness;
using quiz.ModelDb;
using quiz.Ui;
using quiz.Ui.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using quiz.Ui.Areas.Identity;
using quiz.Ui.Security;
using switcher.ThemeSwitcher;

var builder = WebApplication.CreateBuilder(args);

// получение из настроек номера порта backend (Web API).
int backendPortNumber = builder.Configuration.GetValue<int>("BackendPortNumber");
// Получение данных строки подключенияиз настроек.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Добавление в контейнер зависимостей объекта основного контекста БД. Он должен создаваться только на момент использования, чтобы не происходило замеченных ошибок одновременного доступа из разных потоков при логгировании сообщений.
builder.Services.AddDbContext<quizContext>(options => options.UseSqlServer(connectionString), 
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);
// Добавление в контейнер зависимостей объекта контекста БД для управления Identity. Он должен создаваться только на момент использования, чтобы не происходило замеченных ошибок одновременного доступа из разных потоков при логгировании сообщений.
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString),
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);

// Настройка и добавление сервиса для управления Identity.
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
    .AddRoles<IdentityRole>()  // По материалам https://www.c-sharpcorner.com/article/role-based-authorization-in-blazor/
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);  // Добавлено из-за возникновения ошибки NotSupportedException: No IUserTwoFactorTokenProvider<TUser> named 'Default' is registered. По материалам https://stackoverflow.com/questions/59303760/no-iusertwofactortokenprovider-named-default-is-registered-problem-is-adddefa.
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMvc();
builder.Services.AddDevExpressBlazor(options => {
                                         options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
                                         options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
                                     });
// Подключение объекта класса с данными и функциями для аутентификации и авторизации, в том числе контейнера хранения состояний подсистемы аутентификации и авторизации.
builder.Services.AddTransient<ISecurityTokenManageDbDirect, SecurityServiceDbDirect>();

// Подключение сервисов для управления данными через WebAPI.
builder.Services.AddHttpClient<IObjectDataService<TopicModel>, ObjectDataService<TopicModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<QuestionModel>, ObjectDataService<QuestionModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<UserQuestionProgressModel>, ObjectDataService<UserQuestionProgressModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<UserModel>, ObjectDataService<UserModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<RoleModel>, ObjectDataService<RoleModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));

// Настройка служб, обеспечивающих ведение логов.
builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<ILogDbDirect, LogServiceDbDirect>();
builder.Services.AddHttpClient<ILogByHttp, LogServiceByHttp>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));

// #bam_подтверждение_по_email. Отключена необходимость подтверждения нового пользователя по электронной почте.
// Регистрация сервисов для отправки сообщений по электронной почте и СМС. По материалам https://stackoverflow.com/questions/52089864/unable-to-resolve-service-for-type-iemailsender-while-attempting-to-activate-reg
builder.Services.AddTransient<IEmailSender, SendEmailLogic>();
//builder.Services.AddTransient<IEmailSender, SendSmsLogic>();
//// Дополнительная настройка времени ожидания для электронной почты и активности. По материалам https://docs.microsoft.com/ru-ru/aspnet/core/security/authentication/accconfirm?view=aspnetcore-6.0&tabs=visual-studio.
//builder.Services.ConfigureApplicationCookie(o => {
//                                                    o.ExpireTimeSpan = TimeSpan.FromDays(5);
//                                                    o.SlidingExpiration = true;
//                                                });
//builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));

//builder.Services.AddScoped(i => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
////builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ThemeService>();
builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();
// Активация возможности локализации UI. По материалам https://stackoverflow.com/questions/63614887/how-can-i-translate-strings-in-blazor-components-and-app-razor.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped<IStringLocalizer<App>,StringLocalizer<App>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Включение поддержки авторизайии и аутентификации на страницах веб-приложения.
app.UseAuthentication();
app.UseAuthorization();

// Активация возможности локализации контролов DevExpress. 
var supportedCultures = SupportedCultures.Cultures.Select(x => x.Name).ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
