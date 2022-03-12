using aspnet_core.TestProvider;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ITestProvider, TestProvider>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseRouting();

app.Run();
