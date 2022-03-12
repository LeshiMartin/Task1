using AppLogic.FileUploadService;
using AppLogic.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppLogic.BackgroundTask;

internal class FileProcessTask : IExecutableTask
{
  private readonly ILogger<FileProcessTask> _logger;
  private readonly IMediator _mediator;

  public FileProcessTask ( ILogger<FileProcessTask> logger,
    IMediator mediator )
  {
    _logger = logger;
    _mediator = mediator;
  }
  public async Task Execute ( CancellationToken cancellationToken )
  {
    while ( !cancellationToken.IsCancellationRequested )
    {
      _logger.LogInformation ("Executing FileProcessTask at {date}", DateTime.Now);
      await _mediator.Publish (new ProcessFileRequest (), cancellationToken);
      await Task.Delay (TimeSpan.FromMinutes (0.5), cancellationToken);
    }
  }
}