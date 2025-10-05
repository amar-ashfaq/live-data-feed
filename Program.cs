using LiveDataFeed.Services;
using LiveDataFeed.WebSockets;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(); // for HttpClientFactory
builder.Services.AddSingleton<WebSocketHandler>(); // shared instance
builder.Services.AddHostedService<LiveDataService>(); // background polling

var app = builder.Build();

app.UseHttpsRedirection();

// Serve static files from Client folder
app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Client")
    ),
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Client")
    ),
    RequestPath = ""
});

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();

var webSocketHandler = app.Services.GetRequiredService<WebSocketHandler>();
app.Map("/ws", webSocketHandler.HandleAsync);

app.Run();
