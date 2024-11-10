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
// ��������� ������� ���������� ����������� ����������.
ConfigurationManager configuration = builder.Configuration;

// ���������� � ��������� ������������ ������� ��������� ��������� ��. �� ������ ����������� ������ �� ������ �������������, ����� �� ����������� ���������� ������ �������������� ������� �� ������ ������� ��� ������������ ���������.
builder.Services.AddDbContext<quizContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), 
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);
// ���������� � ��������� ������������ ������� ��������� �� ��� ���������� Identity.
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient, 
    ServiceLifetime.Transient);

// ���������� ������� ��� ������������ Identity. �� ���������� https://metanit.com/sharp/aspnet5/16.11.php.
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
    .AddEntityFrameworkStores<IdentityDbContext>();

// ���������� �������� ��� ������ � ������� � ��.
builder.Services.AddScoped<IObjectRepository<TopicModel>, TopicRepository>();
builder.Services.AddScoped<IObjectRepository<QuestionModel>, QuestionRepository>();
builder.Services.AddScoped<IObjectRepository<UserQuestionProgressModel>, UserQuestionProgressRepository>();
builder.Services.AddScoped<IObjectRepository<UserModel>, UserRepository>();
builder.Services.AddScoped<IObjectRepository<RoleModel>, RoleRepository>();
// ���������� � ��������� ������������ ������� ��� ���������� �������� ���� � ��� �����������. ��� ������ ����������� ������ �� ������ ������������� ��� �������������� ������ "� ���� ��������� ���� ������ ������ �������� �� ���������� ���������� ��������. ��� �������, ��� ����������, ����� ��������� ������� ���������� ���� � ��� �� ��������� DbContext. ...", ��������� � https://docs.microsoft.com/ru-ru/ef/core/dbcontext-configuration/#avoiding-dbcontext-threading-issues.
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
