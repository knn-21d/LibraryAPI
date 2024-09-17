
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
using Microsoft.OpenApi.Models;

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
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });
            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!), ServiceLifetime.Singleton);
            builder.Services.AddScoped<CopiesRepository>();
            builder.Services.AddScoped<BooksRepository>();
            builder.Services.AddScoped<CustomersRepository>();
            builder.Services.AddScoped<OrdersRepository>();
            builder.Services.AddScoped<UsersRepository>();
            builder.Services.AddScoped<UserManagementService>();
            builder.Services.AddScoped<StorageManagementService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<PublishersRepository>();
            builder.Services.AddScoped<BooksService>();
            builder.Services.AddScoped<AuthorsRepository>();
            builder.Services.AddScoped<CategoriesRepository>();

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