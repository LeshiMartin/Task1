using AppLogic.BackgroundTask;
using AppLogic.FileServices;
using AppLogic.Interfaces;
using Core;
using DAL;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppLogic;

public static class AppLogicModule
{
  public static IServiceCollection RegisterAppLogicModule ( this IServiceCollection services, IConfiguration configuration )
  {
    GetRepositoryRegister ().RegisterRepository (services, configuration);
    services.AddAutoMapper (typeof (AppLogicModule).Assembly);
    services.AddMediatR (typeof (AppLogicModule).Assembly);
    services.AddScoped<IFileService, FileService> ();
    services.AddScoped<IExecutableTask, FileProcessTask> ();
    services.AddHostedService<ConsumeBackgroundTasks> ();
    return services;
  }

  private static IRegisterRepository GetRepositoryRegister () => new RegisterDALModule ();
}