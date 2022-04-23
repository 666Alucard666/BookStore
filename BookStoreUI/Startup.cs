using DAL;
using Microsoft.EntityFrameworkCore;

namespace BookStoreUI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<AppDbContext>();
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}
