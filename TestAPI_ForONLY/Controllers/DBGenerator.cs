using System.Data;
using TestAPI_ForONLY.Model;

namespace TestAPI_ForONLY.Controllers
{
    public static class DBGenerator
    {
        public static MyContext GetDBFromTable(DataTable dataTable)
        {
            MyContext newDB = new MyContext();

            int rows = dataTable.Rows.Count;

            for (int i = 2; i < rows; i++)
            {

                // проверка на наличие встреченного названия предприятия

                string nameOfDep = dataTable.Rows[i][1].ToString();
                Department dep = newDB.Depts.FirstOrDefault(d => d.Name == nameOfDep);

                if (dep == null)
                {
                    string nameOfParent = dataTable.Rows[i][2].ToString();
                    var depParent = newDB.Depts.FirstOrDefault(d => d.Name == nameOfParent);

                    dep = new Department() { Name = nameOfDep, Parent = depParent };

                    newDB.Depts.Add(dep);
                }

                // проверка на наличие встреченной должности

                string nameOfPost = dataTable.Rows[i][3].ToString();
                Post post = newDB.Posts.FirstOrDefault(d => d.Name == nameOfPost);

                if (post == null)
                {
                    post = new Post() { Name = nameOfPost };

                    newDB.Posts.Add(post);
                }

                // импорт пользователя

                string[] nameOfCust = dataTable.Rows[i][4].ToString().Split(' ');


                Customer customer = new Customer()
                {
                    Surname = nameOfCust[0],
                    Name = nameOfCust[1],
                    Patronymic = nameOfCust[2],
                    Post = post,
                    Dept = dep
                };

                Console.WriteLine(customer);

                i++;
            }

            return newDB;
        }
    }
}
