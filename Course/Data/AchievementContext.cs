using Course.Model.Database.Enum;
using Course.Model.DatabaseTables;
using Course.Model.DatabaseTables.Achievement;
using Course.Model.DatabaseTables.StudentAchivement;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course.Data
{
    public class AchievementContext : DbContext
    {
        public AchievementContext(DbContextOptions<AchievementContext> options) : base(options) { }
        public DbSet<Account> Account { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Achievement> Achievement { get; set; }
        public DbSet<StudentsAchievements> StudentsAchievements { get; set; }

    }

}
       
