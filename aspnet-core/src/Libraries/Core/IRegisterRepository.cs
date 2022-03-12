using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public interface IRegisterRepository
{
  IServiceCollection RegisterRepository(IServiceCollection services,
    IConfiguration configuration );
}