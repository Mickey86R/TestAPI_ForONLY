using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI_ForONLY.Model
{
    public class Customer
    {
        [Column("Customer_ID")]
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }

        public Post Post { get; set; }
        public Department Dept { get; set; }
    }
}