using System.Data;

using TestAPI_ForONLY.Model;

namespace TestAPI_ForONLY.Repository
{
    public static class DBService
    {
        public static async Task<MyContext> GetDBFromTable(DataTable dataTable)
        {
            MyContext newDB = new MyContext();

            int rows = dataTable.Rows.Count;

            for (int i = 2; i < rows; i++)
            {
                // проверка на наличие встреченного названия предприятия

                string nameOfDep = dataTable.Rows[i][1].ToString();
                Department? dep = newDB.Depts.FirstOrDefault(d => d.Name == nameOfDep);

                if (dep == null)
                {
                    string nameOfParent = dataTable.Rows[i][2].ToString();
                    var depParent = newDB.Depts.FirstOrDefault(dp => dp.Name == nameOfParent);

                    dep = new Department() { Name = nameOfDep, Parent = depParent };

                    newDB.Depts.Add(dep);
                }

                // проверка на наличие встреченной должности

                string nameOfPost = dataTable.Rows[i][3].ToString();
                Post? post = newDB.Posts.FirstOrDefault(p => p.Name == nameOfPost);

                if (post == null)
                {
                    post = new Post() { Name = nameOfPost, Dept = dep };

                    newDB.Posts.Add(post);
                }

                // импорт пользователя

                string[] nameOfCust = dataTable.Rows[i][4].ToString().Split(' ');


                Customer newCustomer = new Customer()
                {
                    Surname = nameOfCust[0],
                    Name = nameOfCust[1],
                    Patronymic = nameOfCust[2],
                    Post = post,
                    Dept = dep
                };


                Customer? cust = newDB.Customers.FirstOrDefault(
                                c => c.Name == newCustomer.Name
                                && c.Surname == newCustomer.Surname
                                && c.Patronymic == newCustomer.Patronymic);

                if (cust == null)
                {
                    newDB.Customers.Add(newCustomer);
                }
                else
                {
                    cust.Update(newCustomer);
                    newDB.Customers.Update(cust);
                }

                try
                {
                    newDB.SaveChanges();
                }
                catch { }

                //Console.WriteLine(customer);
            }

            return newDB;
        }

        public static async Task<List<Department>> GetDepartmentsFirstOut(List<Department> depts)
        {
            Department[] newList = new Department[depts.Count];
            depts.CopyTo(newList);

            foreach (var dep in newList)
            {
                dep.Parent = null;
            }

            return newList.ToList();
        }
    }
}
