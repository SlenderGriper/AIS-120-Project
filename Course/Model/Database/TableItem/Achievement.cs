using Course.Model.Database.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Course.Model.DatabaseTables.Achievement
{
    public abstract class Achievement
    {
        [Key]
        public int ID { get; set; }
        public string? WhoСonfirmed { get; set; }
        public string? Description { get; set; }
        public abstract AchiveType AchievementType();


    }
    public class CulturalAchievement : Achievement {
        public override AchiveType AchievementType()
        {
            return AchiveType.Cultural;
        }
    }
    public class ResearchAchievement : Achievement {
        public override AchiveType AchievementType()
        {
            return AchiveType.Research;
        }
    }
    public class SocialAchievement : Achievement {
        public override AchiveType AchievementType()
        {
            return AchiveType.Social;
        }
    }
    public class SportAchievement : Achievement {
        public override AchiveType AchievementType()
        {
            return AchiveType.Sport;
        }
    }
}
