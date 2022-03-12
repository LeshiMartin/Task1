using AppLogic;
using AppLogic.Interfaces;
using aspnet_core.Hubs;

namespace aspnet_core;

public static class ServiceRegistration
{
  public static IServiceCollection RegisterServices ( this IServiceCollection services, IConfiguration configuration )
  {
    var corsSection = configuration.GetSection ("CorsPolicySection");
    var corsPolicy = corsSection.Get<CorsPolicySection> ();
    services.AddCorsPolicy (corsPolicy);
    services.AddSignalR ();
    services.RegisterSwagger ();
    services.RegisterAppLogicModule (configuration);
    services.AddScoped<INotifyFileInProcess, MainHub> ();
    services.AddScoped<INotifyFileIsNotValid, MainHub> ();
    services.AddScoped<INotifyFileIsProcessed, MainHub> ();

    return services;
  }
  private static void AddCorsPolicy ( this IServiceCollection services, CorsPolicySection corsPolicy )
  {
    var origins = corsPolicy.AllowedOrigins.Split (',');
    services.AddCors (x =>
    {
      x.AddPolicy ("Default",
        builder => builder
          .WithOrigins (origins)
          .AllowAnyMethod ()
          .AllowAnyHeader ()
          .AllowCredentials ());
      x.AddPolicy ("AllowAll", c =>
      {
        c.AllowAnyOrigin ()
          .AllowAnyHeader ()
          .AllowAnyMethod ();
      });
    });

  }
}

