using Course.Model.Database.Enum;
using Course.Model.DatabaseTables.Achievement;
using Course.Model.DatabaseTables.StudentAchivement;
using Course.Model.PageItem;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Course.Data
{
    public static class AchievementContextHelper
    {
        public static List<StudentsAchievements> StudentsAchievements(AchiveType typeAchiv,AchievementContext context)
        {
            if (typeAchiv == AchiveType.Social)
            {
                return context.StudentsSocialAchievements.ToList<StudentsAchievements>();
            }
            else if (typeAchiv == AchiveType.Research)
            {
                return  context.StudentsResearchAchievements.ToList<StudentsAchievements>();
            }
            else if (typeAchiv == AchiveType.Cultural)
            {
                return context.StudentsCulturalAchievements.ToList<StudentsAchievements>();
            }
            else  
            {
                return context.StudentsSportAchievements.ToList<StudentsAchievements>();
            }
        }
        public static List<Achievement> Achievement(AchiveType typeAchiv, AchievementContext context)
        {
            if (typeAchiv == AchiveType.Social)
            {
                return context.SocialAchievement.ToList<Achievement>();
            }
            else if (typeAchiv == AchiveType.Research)
            {
                return context.ResearchAchievement.ToList<Achievement>();
            }
            else if (typeAchiv == AchiveType.Cultural)
            {
                return context.CulturalAchievement.ToList<Achievement>();
            }
            else
            {
                return context.SportAchievement.ToList<Achievement>();
            }
        }
    }
}

