using FluentMigrator.Runner;
using MessageService.BLL.Interfaces;
using MessageService.BLL.Services;
using MessageService.DAL;
using MessageService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres() 
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

builder.Services.AddSingleton<IMessageRepository>(new MessageRepository(connectionString));

builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();


app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.MapControllers();

app.Run();