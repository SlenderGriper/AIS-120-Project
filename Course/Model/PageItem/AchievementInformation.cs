using Course.Model.Database.Enum;
using System.ComponentModel.DataAnnotations;

namespace Course.Model.PageItem
{
    public class AchievementInformation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Вид работы")]
        public AchiveType AchievementType { get; set; }


        [Required]
        [Display(Name = "Информация")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "ФИО студента")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Баллы")]
        public double Point { get; set; }
        [Required]
        [Display(Name = "Путь к файлу")]
        public string? FilePath { get; set; }
        public string? WhoСonfirmed { get; set; }
    }
}
