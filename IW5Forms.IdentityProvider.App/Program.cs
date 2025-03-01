﻿using AutoMapper;
using Duende.IdentityServer.Models;
using IW5Forms.Common.Extentions;
using IW5Forms.IdentityProvider.App;
using IW5Forms.IdentityProvider.App.Endpoints;
using IW5Forms.IdentityProvider.App.Installers;
using IW5Forms.IdentityProvider.BL.Installers;
using IW5Forms.IdentityProvider.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddInstaller<IdentityProviderDALInstaller>();
    builder.Services.AddInstaller<IdentityProviderBLInstaller>();
    builder.Services.AddInstaller<IdentityProviderAppInstaller>();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;  // Set SameSite=None
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Ensure HTTPS
        options.Cookie.HttpOnly = true;  // Ensure HttpOnly for security
    });

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder.ConfigureServices();

    var mapper = app.Services.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();

    app.ConfigurePipeline();

    app.MapGroup("api")
        .AllowAnonymous()
        .UseUserEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}