using AppLogic.FileServices;
using AppLogic.Models;
using AutoMapper;
using Core;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AppLogic.FileUploadService;

public record UploadFileRequest ( IFormFile? file ) : IRequest<FileModel>;

internal class UploadFileHandler : IRequestHandler<UploadFileRequest, FileModel>
{
  private readonly IFileRepository _fileRepository;
  private readonly IMapper _mapper;
  private readonly IFileService _fileService;

  public UploadFileHandler ( IFileRepository fileRepository,
    IMapper mapper,
    IFileService fileService )
  {
    _fileRepository = fileRepository;
    _mapper = mapper;
    _fileService = fileService;
  }
  public async Task<FileModel> Handle ( UploadFileRequest request, CancellationToken cancellationToken )
  {
    var file = request.file;
    ValidateFileType (file);
    var route = await _fileService.WriteFileAsync (file!, cancellationToken);
    var uploadFile = new UploadedFile ()
    {
      FileName = Path.GetFileName (route),
      FileUri = route
    };
    await _fileRepository.InsertFileAsync (uploadFile, cancellationToken);
    return _mapper.Map<UploadedFile, FileModel> (uploadFile);

  }

  private static void ValidateFileType ( IFormFile? file )
  {
    if ( file is null )
      throw new ArgumentNullException (nameof (file), "No file present in the request");
    if ( !Path.GetExtension (file.FileName).Contains ("txt") )
      throw new ArgumentException ("The uploaded file is not supported");
  }
}