using Course.Model.Database.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Course.Model.DatabaseTables.StudentAchivement
{
    [Keyless]
    public class StudentsAchievements
    {
       
        public int StudentID { get; set; }
        public int AchievementID { get; set; }
        public double Point { get; set; }
        public string? FilePath { get; set; }

    }

    public class StudentsCulturalAchievements : StudentsAchievements { }
    public class StudentsResearchAchievements  : StudentsAchievements { }
    public class StudentsSocialAchievements  : StudentsAchievements { }
    public class StudentsSportAchievements  : StudentsAchievements { }
}
