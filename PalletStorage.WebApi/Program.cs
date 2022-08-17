using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.HttpLogging;
using FluentValidation;
using FluentValidation.AspNetCore;
using DataContext.Extensions;
using DataContext.Migrations;
using PalletStorage.Repositories.Boxes;
using PalletStorage.Repositories.Pallets;
using PalletStorage.WebApi.Controllers;
using static System.Console;

var builder = WebApplication.CreateBuilder(args);

// Schema and Data migrations
await MigrationManager.MigrateAsync();

// Add services to the container.
builder.Services.AddStorageDataContext();

builder.Services.AddControllers(options =>
    {
        WriteLine("Default output formatters:");
        foreach (IOutputFormatter formatter in options.OutputFormatters)
        {
            if (formatter is not OutputFormatter mediaFormatter)
            {
                WriteLine($"  {formatter.GetType().Name}");
            }
            else // OutputFormatter class has SupportedMediaTypes
            {
                WriteLine("  {0}, Media types: {1}",
                    arg0: mediaFormatter.GetType().Name,
                    arg1: string.Join(", ",
                        mediaFormatter.SupportedMediaTypes));
            }
        }
    })
    .AddXmlDataContractSerializerFormatters()
    .AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PalletStorage Web API", Version = "v1" });
});

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(BoxController)));

// AutoMapper - add all profiles from repositories and controllers
builder.Services.AddAutoMapper(typeof(BoxRepository), typeof(BoxController));

// Repositories
builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IPalletRepository, PalletRepository>();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096; // default is 32k
    options.ResponseBodyLogLimit = 4096; // default is 32k
});

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
