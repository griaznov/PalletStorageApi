using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.HttpLogging;
using FluentValidation;
using DataContext;
using DataContext.Entities.MappingProfiles;
using PalletStorage.WebApi.Models.MappingProfiles;
using PalletStorage.WebApi.Validators.Box;
using PalletStorage.WebApi.Validators.Pallet;
using PalletStorage.Repositories;
using PalletStorage.Repositories.Boxes;
using PalletStorage.Repositories.Pallets;
using static System.Console;
using PalletStorage.WebApi.Validators.Box.RequestValidator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
await builder.Services.AddStorageDataContextAsync();

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
builder.Services.AddValidatorsFromAssemblyContaining<BoxResponseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BoxCreateRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BoxUpdateRequestValidator>();

builder.Services.AddScoped<IBoxRequestValidator, BoxRequestValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<PalletResponseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PalletCreateRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PalletUpdateRequestValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfileApi), typeof(MappingProfileEntity));

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
