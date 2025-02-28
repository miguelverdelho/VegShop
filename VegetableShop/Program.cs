using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using VegetableShop.Base;
using VegetableShop;


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


builder.Services.AddTransient<VegetableShopService>();

var app = builder.Build();

var serviceProvider = app.Services.GetService<IServiceProvider>();
var vegetableShop = serviceProvider?.GetService<VegetableShopService>();

// Use the vegetableShop instance
vegetableShop!.Run();