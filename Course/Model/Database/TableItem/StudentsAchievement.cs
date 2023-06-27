using Course.Model.Database.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Course.Model.DatabaseTables.StudentAchivement
{
    public class StudentsAchievements
    {
        [Key]
      public int ID { get; set; }
        public int StudentID { get; set; }
        public int AchievementID { get; set; }
        public double? Point { get; set; }
       public string? FailMessage { get; set; }

    }

}
