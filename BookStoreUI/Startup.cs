using BLL.Abstractions.ServiceInterfaces;
using BLL.Services;
using Core.Models;
using DAL;
using DAL.Abstractions.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace BookStoreUI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddCors();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200/")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
                options.AddPolicy("AllowAllOrigin", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.WithMethods("GET", "PUT", "POST", "DELETE");
                    builder.AllowAnyHeader();
                });
            });
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings
           .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
           .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
