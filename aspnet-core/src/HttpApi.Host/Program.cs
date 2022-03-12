using aspnet_core;
using aspnet_core.Hubs;

var builder = WebApplication.CreateBuilder (args);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build ();
app.UseSwaggerGen ();
app.UseCors ("Default");
app.UseCors ("AllowAll");
app.MapHub<MainHub>("/hub");
app.UseRouting ();

app.Run ();
