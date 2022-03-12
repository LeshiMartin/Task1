using AppLogic.BackgroundTask;
using AppLogic.FileServices;
using AppLogic.Interfaces;
using Core;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace AppLogic;

public static class AppLogicModule
{
  public static IServiceCollection RegisterAppLogicModule (this IServiceCollection services, IConfiguration configuration )
  {

    //var types = AppDomain.CurrentDomain.GetAssemblies ()
    //  .SelectMany (s => s.GetTypes ())
    //  .Where (p => typeof (IRegisterRepository).IsAssignableFrom (p)).ToArray ();
    //if ( !types.Any () )
    //  throw new ArgumentException ("You must reference a DAL Project for this app to work");

    //var firstType = types.First ();
    //dynamic first =  Activator.CreateInstance (firstType)!;
    var dalModule = new RegisterDALModule();
    dalModule.RegisterRepository (services, configuration);

    services.AddAutoMapper (typeof (AppLogicModule).Assembly);
    services.AddMediatR (typeof (AppLogicModule).Assembly);
    services.AddScoped<IFileService, FileService> ();
    services.AddScoped<IExecutableTask, FileProcessTask> ();
    services.AddHostedService<ConsumeBackgroundTasks> ();
    return services;
  }
}