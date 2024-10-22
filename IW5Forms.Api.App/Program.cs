using AutoMapper;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Api.DAL.EF.Repositories;
using IW5Forms.Common.Models;
using Microsoft.EntityFrameworkCore;
using IW5Forms.Api.DAL.EF.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace IW5Forms.Api.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<SeedScript>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<FormsDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection"));
            });
            builder.Services.AddAutoMapper(typeof(EntityBase));
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //SeedData(app);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            //var mapper = app.Services.GetService<IMapper>();
            //var userRepo = app.Services.GetService<UserRepository>();
            //var user = mapper.Map<List<UserModel>>(userRepo.GetAll());
            //Console.WriteLine("count: " + user.Count + '\0');

            app.Run();

            
        }

        static void SeedData(IHost app)
        {
            var facorry = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = facorry.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<SeedScript>();
                service.SeedData();
            }
        }
    }
}
