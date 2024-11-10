using Microsoft.EntityFrameworkCore;

namespace quiz.ModelDb
{
    /// <summary>
    /// Этот DbContext используется для управления пользователями, ролями и т.д. (Identity). Не получается объединить с основным quizContext так как он перестраивается заново после выполнения команды Scaffold-DbContext.
    /// </summary>
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
    }
}