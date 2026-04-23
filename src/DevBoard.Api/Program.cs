using DevBoard.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline();
app.MapApiEndpoints();

app.Run();
