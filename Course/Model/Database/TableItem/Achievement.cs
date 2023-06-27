using Course.Model.Database.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Course.Model.DatabaseTables.Achievement
{
    public class Achievement
    {
        [Key]
        public int ID { get; set; }
        public string? WhoСonfirmed { get; set; }
        public string? Description { get; set; }
        public AchiveType AchievementType { get; set; }
        public string? FilePath { get; set; }
        public AchiveStatus Status { get; set; }
    }
}
