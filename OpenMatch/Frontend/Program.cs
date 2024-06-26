using Frontend.Services;
using Frontend.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Adding logging to Dependency Injection
var logger = LoggerFactory.Create(
    logBuilder => logBuilder.AddConsole(
        configuration =>
        {
            configuration.TimestampFormat = "[HH:mm:ss] ";
        })
    )
    .CreateLogger("MatchFunction");
builder.Services.AddSingleton<ILogger>(logger);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v0.1", new OpenApiInfo { Title = "Matchmaker", Version = "v0.1" });
});


var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v0.1/swagger.json", "Matchmaker-v0.1");
    });
}

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, ex.Message);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
    }
});

app.MapControllers();

app.MapGet("/", () => "Open Match Frontend");

app.Run();
