using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VegetableShop.Interfaces;


Console.OutputEncoding = System.Text.Encoding.UTF8;// to print euro sign

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
var env = builder.Environment;

builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //load base settings
    .AddEnvironmentVariables();

// Register interfaces
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IBaseSingleton>()
    .AddClasses(classes => classes.AssignableTo<IBaseSingleton>())
    .AsImplementedInterfaces()
    .WithSingletonLifetime());

//Set up global error handling

var app = builder.Build();

var serviceProvider = app.Services.GetService<IServiceProvider>();
var vegetableShop = serviceProvider?.GetService<IVegetableShopOrchestratorService>();

// Use the vegetableShop instance
vegetableShop!.Run();