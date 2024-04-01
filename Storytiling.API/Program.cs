using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Storytiling.ApplicationCore;
using Storytiling.ApplicationCore.Settings;
using Storytiling.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register Infrastructure
builder.Services.RegisterInfrastructureServices(builder.Configuration); ;

var app = builder.Build();
// Configure global exception handler to log exceptions and return error response
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var logger = exceptionHandlerApp.ApplicationServices.GetRequiredService<ILogger<Program>>();

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;

        await context.Response.WriteAsync("An exception was thrown.");

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        logger.LogError(exceptionHandlerPathFeature!.Error, exceptionHandlerPathFeature.Path);
    });
});
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
