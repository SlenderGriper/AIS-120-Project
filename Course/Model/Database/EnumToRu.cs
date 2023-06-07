using Course.Model.Database.Enum;

namespace Course.Model.Database
{
    public static class EnumToRu
    {
        static public string GetRuAchiveType(AchiveType achiveType)
        {
            switch (achiveType)
            {
                case AchiveType.Sport:
                    return "Спортивная";
                case AchiveType.Social:
                    return "Общественная";
                case AchiveType.Cultural:
                    return "Культурно-творческая";
                case AchiveType.Research:
                    return "Научно-исследовательская";
            }
            return "";
        }
        static public string GetRuRoles(Roles role)
        {
            switch (role)
            {
                case Roles.Administrator:
                    return "Администратор";
                case Roles.Staff:
                    return "Сотрудник университета";
                case Roles.Student:
                    return "Студент";
            }
            return "";
        }
    }
}
