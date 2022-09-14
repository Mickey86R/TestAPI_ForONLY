using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using TestAPI_ForONLY.Model;

namespace TestAPI_ForONLY.Controllers
{
    [Route("/MyCar_API/")]
    [ApiController]
    public class MyController : ControllerBase
    {
        MyContext db;

        public MyController(MyContext context)
        {
            this.db = context;
        }


        [HttpGet]
        public string Get()
        {
            string s = "Всё работает";

            return s;
        }

        [Route("/API/ImportFromFile")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> ImportFromFile()
        {
            using ()
            Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
            ex.Visible = false;
            ex.Workbooks.Open(@"C:\Users\Михаил\Desktop\Тестовые данные.xlsx");
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);

            MyContext newDB = new MyContext();

            int i = 1;
            while (sheet.Cells[i, 0].ToString() != "")
            {
                // проверка на наличие встреченного названия предприятия

                string nameOfDep = sheet.Cells[i, 1].ToString();
                Department dep = newDB.Depts.FirstOrDefault(d => d.Name == nameOfDep);

                if (dep == null)
                {
                    string nameOfParent = sheet.Cells[i, 2].ToString();
                    var depParent = newDB.Depts.FirstOrDefault(d => d.Name == nameOfParent);

                    dep = new Department() { Name = nameOfDep, Parent = depParent };

                    newDB.Depts.Add(dep);
                }

                // проверка на наличие встреченной должности

                string nameOfPost = sheet.Cells[i, 3].ToString();
                Post post = newDB.Posts.FirstOrDefault(d => d.Name == nameOfPost);

                if (post == null)
                {
                    post = new Post() { Name = nameOfPost};

                    newDB.Posts.Add(post);
                }

                // импорт пользователя

                string[] nameOfCust = sheet.Cells[i, 4].ToString().Split(' ');


                Customer customer = new Customer()
                {
                    Surname = nameOfCust[0],
                    Name = nameOfCust[1],
                    Patronymic = nameOfCust[2],
                    Post = post,
                    Dept = dep
                };

                i++;
            }

            return newDB.Customers;
        }

        [Route("/MyCar_API/GetAutos")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAutos()
        {
            return new List<Customer>();
        }

        [Route("/MyCar_API/GetAutoFromUser/{id}")]
        [HttpGet]
        public IEnumerable<Customer> GetAutoFromUser(int id)
        {
            return new List<Customer>();
        }

        [Route("/MyCar_API/GetUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetUsers()
        {
            return await this.db.Customers.ToListAsync();
        }

        [Route("/MyCar_API/GetUser/{id}")]
        public async Task<ActionResult<Customer>> GetUserFromID(int id)
        {
            Customer user = await db.Customers.FirstOrDefaultAsync(x => x.ID == id); // Получить пользователя из БД с заданым id

            if (user == null)
                return NotFound();

            return user;
        }

        //--------------------------------------POST

        [HttpPost("/API/PostFromFile/")]
        public async Task<ActionResult> PostUser(Customer newUser)
        {
            Customer user = db.Customers.FirstOrDefault(x => x.ID == newUser.ID);

            db.Customers.Add(user);
            db.SaveChanges();

            return Ok(user);
        }

        [HttpPost("/MyCar_API/PostUser/")]
        public async Task<ActionResult> PostUser1(Customer newUser)
        {
            Customer user = db.Customers.FirstOrDefault(x => x.ID == newUser.ID);

            db.Customers.Add(user);
            db.SaveChanges();

            return Ok(user);
        }



        //--------------------------------------PUT

        [HttpPut("/MyCar_API/PutUser/")]
        public async Task<ActionResult> PutUser(Customer newUser)
        {
            Customer user = db.Customers.FirstOrDefault(x => x.ID == newUser.ID);

            db.Customers.Update(user);
            db.SaveChanges();

            return Ok(user);
        }

        //--------------------------------------DELETE

        [HttpDelete("/MyCar_API/DelUser/{id}")]
        public async Task<ActionResult<Customer>> DelUser(int id)
        {
            Customer user = db.Customers.FirstOrDefault(x => x.ID == id);
            db.Customers.Remove(user);

            await db.SaveChangesAsync();

            return Ok(user);
        }
    }
}
