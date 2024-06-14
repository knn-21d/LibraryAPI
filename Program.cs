
using LibraryAPI.Data;
using LibraryAPI.Data.Repositories;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace LibraryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
            builder.Services.AddSingleton<CopiesRepository>();
            builder.Services.AddSingleton<BooksRepository>();
            builder.Services.AddSingleton<CustomersRepository>();
            builder.Services.AddSingleton<OrdersRepository>();
            builder.Services.AddSingleton<UsersRepository>();
            builder.Services.AddSingleton<UserManagementService>();
            builder.Services.AddSingleton<StorageManagementService>();
            builder.Services.AddSingleton<OrderService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}