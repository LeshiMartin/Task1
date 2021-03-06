using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL;
public class RegisterDALModule : IRegisterRepository
{
  public IServiceCollection RegisterRepository ( IServiceCollection services, IConfiguration configuration )
  {
    services.AddDbContextFactory<CoreDbContext> (builder =>
       builder.UseSqlServer (configuration.GetConnectionString ("Default"),
         x=>x.MigrationsAssembly(typeof(RegisterDALModule).Assembly.FullName)));
    services.AddScoped<IFileRepository, FileRepository> ();
    return services;
  }
}
