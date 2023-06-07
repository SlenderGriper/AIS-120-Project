using System.ComponentModel.DataAnnotations;

namespace Course.Model.Database.Enum
{

    public enum AchiveType
    {
        [Display(Name = "Спортивная")]
        Sport,
        [Display(Name = "Общественная")]
        Social,
        [Display(Name = "Культурно-творческая")]
        Cultural,
        [Display(Name = "Научно-исследовательские")]
        Research
    }
}
