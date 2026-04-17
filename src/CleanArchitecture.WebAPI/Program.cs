using RecipeManagement.Application;
using RecipeManagement.Infrastructure;
using RecipeManagement.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDefaults(string.Empty);

builder.Services.AddApplication();
builder.AddInfrastructure();

var app = builder.Build();

app.MapControllers();
app.UseDefaults();

app.Run();