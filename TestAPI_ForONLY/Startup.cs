using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TestAPI_ForONLY.Model;

namespace TestAPI_ForONLY
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string con = "connectionstring";

            // устанавливаем контекст данных
            services.AddDbContext<MyContext>(options => options.UseNpgsql(con));

            services.AddControllers(); // используем контроллеры без представлений
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // подключаем маршрутизацию на контроллеры
            });
        }
    }
}