using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI_ForONLY.Model
{
    public class Post
    {
        [Column("Post_ID")]
        public int ID { get; set; }
        public string Name { get; set; }

        [Column("Dept_ID")]
        public int DeptID { get; set; }

        public Department Dept { get; set; }
    }
}
