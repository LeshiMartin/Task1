using Microsoft.OpenApi.Models;

namespace aspnet_core;

public static class SwaggerGenRegister
{
  public static IServiceCollection RegisterSwagger ( this IServiceCollection services )
  {
    var contact = new OpenApiContact ()
    {
      Name = "Martin Leshi",
      Email = "leshi1martin@gmail.com",
      Url = new Uri ("http://www.example.com")
    };

    var license = new OpenApiLicense ()
    {
      Name = "My License",
      Url = new Uri ("http://www.example.com")
    };

    var info = new OpenApiInfo ()
    {
      Version = "v1",
      Title = "Swagger Squadron Task1",
      TermsOfService = new Uri ("http://www.example.com"),
      Contact = contact,
      License = license
    };

    services.AddEndpointsApiExplorer ();

    services.AddSwaggerGen (o =>
    {
      o.SwaggerDoc ("v1", info);
    });
    return services;
  }

  public static void UseSwaggerGen(this WebApplication app)
  {
    app.UseSwagger ();

    app.UseSwaggerUI (c =>
    {
      c.SwaggerEndpoint ("/swagger/v1/swagger.json",
        "Swagger Squadron Task1 v1");
    });
  }
}