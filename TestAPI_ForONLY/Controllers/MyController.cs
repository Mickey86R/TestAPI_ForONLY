using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Data;
using Newtonsoft.Json;

using TestAPI_ForONLY.Model;
using TestAPI_ForONLY.Repository;

namespace TestAPI_ForONLY.Controllers
{
    [Route("/API/")]
    [ApiController]
    public class MyController : ControllerBase
    {
        MyContext db;

        public MyController(MyContext context)
        {
            this.db = context;
        }

        

        // CRUD для получения оргструктуры
        [Route("/API/GetDepartments")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var depts = await db.Depts.ToListAsync();

            depts = await DBService.GetDepartmentsFirstOut(depts);

            return depts;
        }

        // CRUD для получения оргструктуры с фильтром родительского объекта
        [Route("/API/GetDepartmentsWithFilter")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartmentsWithFilter(string? parent)
        {
            List<Department>? depts;

            if (parent == null)
            {
                depts = await db.Depts.Where(d => d.ParentID == null).ToListAsync();
            }
            else
            {
                depts = await db.Depts.Where(d => (d.Parent != null) && d.Parent.Name == parent).ToListAsync();
            }

            depts = await DBService.GetDepartmentsFirstOut(depts);

            return depts;
        }

        // эндпоинт для вывода информации по количеству сотрудников в каждом отделе и количеству позиций в этом отделе
        [Route("/API/GetInfoFromDepts")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeptInfo>>> GetInfoFromDepts()
        {
            var deptList = await db.Depts.ToListAsync();
            var listInfo = new List<DeptInfo>();

            foreach (var item in deptList)
            {
                string name = item.Name;
                int postCount = db.Posts.Where(p => p.DeptID == item.ID).Count();
                int custCount = db.Customers.Where(c => c.DeptID == item.ID).Count();
                
                listInfo.Add(new DeptInfo(name, postCount, custCount));
            }

            return listInfo;
        }


        [Route("/API/GetPosts")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(int id)
        {
            return await db.Posts.ToListAsync();
        }

        [Route("/API/GetCustomers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await this.db.Customers.ToListAsync();
        }

        //--------------------------------------POST


        // парсинг данных из .xlsx файла
        [HttpPost("/API/ImportFromFile/")]
        public async Task<ActionResult<IEnumerable<Customer>>> ImportFromFile(string path)
        {
            //string path = @"C:\Users\User\Downloads\Тестовые данные (1).xlsx";

            DataTable dataTable = await FileManager.GetTableFromFile(path);

            MyContext newDB = await DBService.GetDBFromTable(dataTable);

            if (db.Posts.Count() == 0)
            {
                db = newDB;
                db.SaveChanges();
            }

            return await newDB.Customers.ToListAsync();
        }


        //--------------------------------------PUT



        //--------------------------------------DELETE


    }
}
