using Quartz;
using WebapiWithQuartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQuartzServices<HaveSomeCoffeJob>(builder.Configuration);

var app = builder.Build();

app.Run();
