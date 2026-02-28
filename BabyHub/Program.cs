using AutoMapper;
using BabyHub.Application.Contracts.Patients;
using BabyHub.Application.Patients;
using BabyHub.Domain.Patients;
using BabyHub.EntityFrameworkCore;
using BabyHub.EntityFrameworkCore.Patients;
using Microsoft.EntityFrameworkCore;
using Sot.ProductService;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using BabyHub.Domain.Shared.Exceptions;
using System.ComponentModel;
using BabyHub.Utils;

namespace BabyHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BabyHubDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IPatientAppService, PatientAppService>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddAutoMapper(typeof(ApplicationAutoMapperProfile));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    );

                    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
                });

            builder.Services.AddValidatorsFromAssemblyContaining<PatientCreateDtoValidator>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "BabyHub API",
                    Version = "v1",
                    Description = "REST API for managing patients"
                });

                options.MapType<DateTime>(() => new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "string",
                    Example = new Microsoft.OpenApi.Any.OpenApiString("2024-01-13T18:25:43"),
                    Description = "Format: yyyy-MM-ddTHH:mm:ss. Timezone is not supported."
                });

                options.SupportNonNullableReferenceTypes();
                options.UseInlineDefinitionsForEnums();
                var xmlFiles = new[]
                {
                    "BabyHub.HttpApi.xml",
                    "BabyHub.Application.Contracts.xml"
                };

                foreach (var xmlFile in xmlFiles)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                }
            });

            var app = builder.Build();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = ex switch
                    {
                        NotFoundException => 404,
                        ArgumentException => 400,
                        _ => 500
                    };
                    await context.Response.WriteAsJsonAsync(new { error = ex?.Message });
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseAuthorization();
            app.MapControllers();

            ApplyMigrations(app);

            app.Run();
        }

        static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BabyHubDbContext>();

            if (app.Environment.IsEnvironment("Docker"))
            {
                var retries = 5;
                while (retries > 0)
                {
                    try
                    {
                        db.Database.Migrate();
                        break;
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        Console.WriteLine($"Migration failed, retries left: {retries}. Error: {ex.Message}");
                        if (retries == 0) throw;
                        Thread.Sleep(5000);
                    }
                }
            }
            else
            {
                db.Database.Migrate();
            }
        }
    }
}