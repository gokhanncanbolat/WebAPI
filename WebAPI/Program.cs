using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services.Contracts;
using WebAPI.Extension;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlgo.config"));

        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        })
            .AddCustomCsvFormatter()
            .AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        //builder.Services.AddScoped<ValidationFilterAttribute>();  // Ioc kaydi

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureSqlContext(builder.Configuration);
        builder.Services.ConfigureRepositoryManager();
        builder.Services.ConfigureServiceManager();
        builder.Services.ConfigureLoggerService();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.ConfigureActionFilters();

        builder.Services.ConfigureCors();

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILoggerService>();
        app.ConfigureExceptionHandler(logger);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}