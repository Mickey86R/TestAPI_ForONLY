using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI_ForONLY.Model
{
    public class Department
    {
        [Column("Dept_ID")]
        public int ID { get; set; }
        public string Name { get; set; }

        public int? ParentID { get; set; }
        public Department? Parent { get; set; }
    }
}
