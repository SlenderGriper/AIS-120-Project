using System.ComponentModel.DataAnnotations;

namespace Course.Model.DatabaseTables
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public int AccountID { get; set;}
    }
}
