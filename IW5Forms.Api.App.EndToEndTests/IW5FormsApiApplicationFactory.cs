using System.Reflection;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.API.DAL;
using IW5Forms.Common.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace IW5Forms.Api.App.EndToEndTests;

public class IW5FormsApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(collection =>
        {
            var controllerAssemblyName = typeof(Program).Assembly.FullName;
            collection
                .AddMvc()
                .AddApplicationPart(Assembly.Load(controllerAssemblyName));

            var sp = collection.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db =
                    scope.ServiceProvider.GetRequiredService<FormsDbContext>();
                db.Database.Migrate();
                ClearDatabase(db);
                SeedDatabase(db);
            }
        });

        return base.CreateHost(builder);
    }

    private void ClearDatabase(FormsDbContext db)
    {
        db.Forms.RemoveRange(db.Forms);
        db.SaveChanges();
    }

    private void SeedDatabase(FormsDbContext context)
    {
        context.Forms.Add(new FormEntity {
            Id = new("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
            Name = "Testing form",
            BeginTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(3),
            Incognito = true,
            SingleTry = true,
            Questions = new List<QuestionEntity>()
            {
                new QuestionEntity()
                {
                    Id = new("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
                    QuestionType = QuestionTypes.NumericValue,
                    Text = "What's the value of Pi?",
                }
            }
        });

        context.SaveChanges();
    }
}
