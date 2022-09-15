using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using TestAPI_ForONLY.Model;
using System.Data;

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
            string path = @"C:\Users\Михаил\Desktop\Fuck.xlsx";

            DataTable dataTable = FileManager.GetTableFromFile(path);

            MyContext newDB = DBGenerator.GetDBFromTable(dataTable);

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
