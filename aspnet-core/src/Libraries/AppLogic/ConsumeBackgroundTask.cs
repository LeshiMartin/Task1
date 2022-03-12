using AppLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppLogic;
internal class ConsumeBackgroundTasks : BackgroundService
{
  private readonly IServiceProvider _services;
  private readonly ILogger<ConsumeBackgroundTasks> _logger;

  public ConsumeBackgroundTasks ( IServiceProvider services, ILogger<ConsumeBackgroundTasks> logger )
  {
    _services = services;
    _logger = logger;
  }
  protected override async Task ExecuteAsync ( CancellationToken stoppingToken )
  {
    _logger.LogInformation (
      "Consume Scoped Service Hosted Service running.");

    await Execute (stoppingToken);
  }

  private async Task Execute ( CancellationToken cancellationToken )
  {
    _logger.LogInformation (
      "Consume Scoped Service Hosted Service is working.");

    using var scope = _services.CreateScope ();
    var serviceProvider = scope.ServiceProvider;
    var task = serviceProvider.GetRequiredService<IExecutableTask> ();
    await task.Execute (cancellationToken);
  }

  public override async Task StopAsync ( CancellationToken cancellationToken )
  {
    _logger.LogInformation (
      "Consume Scoped Service Hosted Service is stopping.");

    await base.StopAsync (cancellationToken);
  }
}
