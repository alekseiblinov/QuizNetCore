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

// ��������� �� �������� ������ ����� backend (Web API).
int backendPortNumber = builder.Configuration.GetValue<int>("BackendPortNumber");
// ��������� ������ ������ ������������� ��������.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ���������� � ��������� ������������ ������� ��������� ��������� ��. �� ������ ����������� ������ �� ������ �������������, ����� �� ����������� ���������� ������ �������������� ������� �� ������ ������� ��� ������������ ���������.
builder.Services.AddDbContext<quizContext>(options => options.UseSqlServer(connectionString), 
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);
// ���������� � ��������� ������������ ������� ��������� �� ��� ���������� Identity. �� ������ ����������� ������ �� ������ �������������, ����� �� ����������� ���������� ������ �������������� ������� �� ������ ������� ��� ������������ ���������.
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString),
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);

// ��������� � ���������� ������� ��� ���������� Identity.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                                                         {
                                                             options.SignIn.RequireConfirmedAccount = false;     // #bam_�������������_��_email. ��������� ������������� ������������� ������ ������������ �� ����������� �����.
                                                             // ��������� ���������� � ��������� ������. �� ���������� https://metanit.com/sharp/aspnet5/16.9.php.
                                                             options.Password.RequiredLength = 5;               // ����������� �����.
                                                             options.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������.
                                                             options.Password.RequireLowercase = false;         // ��������� �� ������� � ������ ��������.
                                                             options.Password.RequireUppercase = false;         // ��������� �� ������� � ������� ��������.
                                                             options.Password.RequireDigit = false;             // ��������� �� �����.
                                                             // ��������� ���������� � ������ ������������. �� ���������� https://metanit.com/sharp/aspnet5/16.10.php.
                                                             options.User.RequireUniqueEmail = true;                                    // ��������� �� ������������ email.
                                                             options.User.AllowedUserNameCharacters = "._ @1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz�����Ũ����������������������������������������������������������";   // ���������� �������.
                                                         }) 
    .AddRoles<IdentityRole>()  // �� ���������� https://www.c-sharpcorner.com/article/role-based-authorization-in-blazor/
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);  // ��������� ��-�� ������������� ������ NotSupportedException: No IUserTwoFactorTokenProvider<TUser> named 'Default' is registered. �� ���������� https://stackoverflow.com/questions/59303760/no-iusertwofactortokenprovider-named-default-is-registered-problem-is-adddefa.
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMvc();
builder.Services.AddDevExpressBlazor(options => {
                                         options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
                                         options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
                                     });
// ����������� ������� ������ � ������� � ��������� ��� �������������� � �����������, � ��� ����� ���������� �������� ��������� ���������� �������������� � �����������.
builder.Services.AddTransient<ISecurityTokenManageDbDirect, SecurityServiceDbDirect>();

// ����������� �������� ��� ���������� ������� ����� WebAPI.
builder.Services.AddHttpClient<IObjectDataService<TopicModel>, ObjectDataService<TopicModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<QuestionModel>, ObjectDataService<QuestionModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<UserQuestionProgressModel>, ObjectDataService<UserQuestionProgressModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<UserModel>, ObjectDataService<UserModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));
builder.Services.AddHttpClient<IObjectDataService<RoleModel>, ObjectDataService<RoleModel>>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));

// ��������� �����, �������������� ������� �����.
builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<ILogDbDirect, LogServiceDbDirect>();
builder.Services.AddHttpClient<ILogByHttp, LogServiceByHttp>(client => client.BaseAddress = new Uri($"https://localhost:{backendPortNumber}/"));

// #bam_�������������_��_email. ��������� ������������� ������������� ������ ������������ �� ����������� �����.
// ����������� �������� ��� �������� ��������� �� ����������� ����� � ���. �� ���������� https://stackoverflow.com/questions/52089864/unable-to-resolve-service-for-type-iemailsender-while-attempting-to-activate-reg
builder.Services.AddTransient<IEmailSender, SendEmailLogic>();
//builder.Services.AddTransient<IEmailSender, SendSmsLogic>();
//// �������������� ��������� ������� �������� ��� ����������� ����� � ����������. �� ���������� https://docs.microsoft.com/ru-ru/aspnet/core/security/authentication/accconfirm?view=aspnetcore-6.0&tabs=visual-studio.
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
// ��������� ����������� ����������� UI. �� ���������� https://stackoverflow.com/questions/63614887/how-can-i-translate-strings-in-blazor-components-and-app-razor.
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

// ��������� ��������� ����������� � �������������� �� ��������� ���-����������.
app.UseAuthentication();
app.UseAuthorization();

// ��������� ����������� ����������� ��������� DevExpress. 
var supportedCultures = SupportedCultures.Cultures.Select(x => x.Name).ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
