using DevBoard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline();
app.MapApiEndpoints();
await app.InitializeDatabaseAsync();

app.Run();

public partial class Program;
