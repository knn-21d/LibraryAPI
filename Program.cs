
using LibraryAPI.Data;
using LibraryAPI.Data.Repositories;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibraryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            if (jwtKey is not null)
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = jwtIssuer,
                         ValidAudience = jwtIssuer,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                     };
                 });
            }


            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!), ServiceLifetime.Singleton);
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