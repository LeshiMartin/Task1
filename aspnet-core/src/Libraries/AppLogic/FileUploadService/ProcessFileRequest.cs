using AppLogic.FileServices;
using AppLogic.Interfaces;
using AppLogic.Models;
using Core;
using Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppLogic.FileUploadService;

public class ProcessFileRequest : INotification
{ }

public class ProcessFileHandler : INotificationHandler<ProcessFileRequest>
{
  private readonly IFileRepository _fileRepository;
  private readonly IFileService _fileService;
  private readonly INotifyFileIsNotValid _fileIsNotValid;
  private readonly INotifyFileInProcess _fileInProcess;
  private readonly INotifyFileIsProcessed _fileIsProcessed;
  private readonly ILogger<ProcessFileHandler> _logger;

  public ProcessFileHandler ( IFileRepository fileRepository,
    IFileService fileService,
    INotifyFileIsNotValid fileIsNotValid,
    INotifyFileInProcess fileInProcess,
    INotifyFileIsProcessed fileIsProcessed,
    ILogger<ProcessFileHandler> logger )
  {
    _fileRepository = fileRepository;
    _fileService = fileService;
    _fileIsNotValid = fileIsNotValid;
    _fileInProcess = fileInProcess;
    _fileIsProcessed = fileIsProcessed;
    _logger = logger;
  }
  public async Task Handle ( ProcessFileRequest notification, CancellationToken cancellationToken )
  {
    var file = await _fileRepository.GetFirstNotProcessedFile (cancellationToken);
      if ( file is null )
        return;
    try
    {
      _logger.LogInformation ("Execution ProcessFile Handle");

      var fileContent = await _fileService.ReadFileAsync (file.FileUri, cancellationToken);
      var validContent = ValidateFileContent (fileContent);
      await _fileInProcess.FileIsInProcess (file.Id);
      var rows = validContent.Select (x =>
        new FileRow (x[ 0 ], x[ 2 ], int.Parse (x[ 1 ]))
        { UploadedFileId = file.Id });
      await _fileRepository.InsertFileRowsAsync (rows, cancellationToken);
      await _fileIsProcessed.FileIsProcessed (file.Id);
      await _fileService.RemoveFileAsync(file.FileUri, cancellationToken);
    }
    catch ( ArgumentException exc )
    {
      _logger.LogError (exc, "{Message}", exc.Message);
      await SetFileAsInValid (cancellationToken, file);
    }
    catch ( Exception exc )
    {
      _logger.LogError (exc, "{Message}", exc.Message);
    }
  }

  private IEnumerable<string[]> ValidateFileContent ( string strContent )
  {
    if ( string.IsNullOrEmpty (strContent) )
      throw new ArgumentException ("The file passed is empty");
    try
    {
      var dataContent = new FileContent (strContent).ProcessContent ();
      if ( dataContent is null )
        throw new ArgumentException ("The content of the file is not valid");
      var validContent = dataContent.Where (x => x.Length == 3 && int.TryParse (x[ 1 ], out _)).ToArray ();
      if ( validContent is null )
        throw new ArgumentException ("The content of the file is not valid");

      return validContent;

    }
    catch ( Exception exc )
    {
      _logger.LogError (exc, "{Message}", exc.Message);
      throw new ArgumentException ("The file is not valid");
    }
  }

  private async Task SetFileAsInValid ( CancellationToken cancellationToken, UploadedFile file )
  {
    file.FileStatus = (int) UploadFileStatus.InValid;
    await _fileRepository.UpdateFileAsync (file, cancellationToken);
    await _fileIsNotValid.FileIsNotValid (file.Id);
  }
}