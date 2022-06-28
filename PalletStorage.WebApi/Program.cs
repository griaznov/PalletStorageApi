using DataContext;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Swashbuckle.AspNetCore.SwaggerUI; // SubmitMethod
using Microsoft.AspNetCore.HttpLogging;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Models.Converters;
using DataContext.Models.Converters;
using FluentValidation;
using FluentValidation.Results;
using PalletStorage.WebApi.Validators;

// HttpLoggingFields

using static System.Console;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
await builder.Services.AddStorageDataContextAsync();

//builder.Services.AddControllers();
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

builder.Services.AddValidatorsFromAssemblyContaining<BoxValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PalletValidator>();

builder.Services.AddAutoMapper(typeof(MappingProfileApi));
builder.Services.AddAutoMapper(typeof(MappingProfileEf));

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
