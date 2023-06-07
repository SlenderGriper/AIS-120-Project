using Course.Model.Database.Enum;
using System.ComponentModel.DataAnnotations;

namespace Course.Model.DatabaseTables
{
    public class Account
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string? FullName { get; set; }
        [Required]
        [Display(Name = "Права доступа")]
        public Roles? AccessRights { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string? HashPassword { get; set; }
    }
}
