using System.ComponentModel.DataAnnotations;

namespace Course.Model.Database.Enum
{

    public enum Roles
    {
        [Display(Name = "Студент")]
        Student,
        [Display(Name = "Администратор")]
        Administrator,
        [Display(Name = "Сотрудник университета")]
        Staff
    }
}
