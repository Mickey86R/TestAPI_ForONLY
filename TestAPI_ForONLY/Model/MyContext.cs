using Microsoft.EntityFrameworkCore;

namespace TestAPI_ForONLY.Model
{
    public class MyContext : DbContext
    {
        public DbSet<Department> Depts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public MyContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=newUser;Password=123qweASD");
        }
    }
}
