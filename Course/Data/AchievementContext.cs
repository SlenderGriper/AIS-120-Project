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
        public DbSet<CulturalAchievement> CulturalAchievement { get; set; }
        public DbSet<ResearchAchievement> ResearchAchievement { get; set; }
        public DbSet<SocialAchievement> SocialAchievement { get; set; }
        public DbSet<SportAchievement> SportAchievement { get; set; }
        public DbSet<StudentsCulturalAchievements> StudentsCulturalAchievements { get; set; }
        public DbSet<StudentsResearchAchievements> StudentsResearchAchievements { get; set; }
        public DbSet<StudentsSocialAchievements> StudentsSocialAchievements { get; set; }
        public DbSet<StudentsSportAchievements> StudentsSportAchievements { get; set; }

    }

}
       
