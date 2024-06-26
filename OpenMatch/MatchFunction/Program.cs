using Matchmaker.Configuration;

var builder = WebApplication.CreateBuilder(args);

var logger = LoggerFactory.Create(
    logBuilder => logBuilder.AddConsole(
        configuration =>
        {
            configuration.TimestampFormat = "[HH:mm:ss] ";
        })
    )
    .CreateLogger("MatchFunction");
builder.Services.AddSingleton<ILogger>(logger);

var myConfig = ConfigurationLoader.LoadConfiguration();
builder.Services.AddSingleton(myConfig);
Core.MatchFunction.Initialize(myConfig);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Handle Exceptions
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, ex.Message);
        await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
    }
});

// Handle the creation of match proposals
app.MapPost("/v1/matchfunction:run", Core.MatchFunction.Handle);

app.Run();
