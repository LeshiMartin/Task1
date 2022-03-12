using AppLogic.Models;
using AutoMapper;
using Core;
using Core.Entities;
using MediatR;

namespace AppLogic.FileUploadService;

public record GetFilesRequest () : IRequest<IEnumerable<FileModel>>;

internal class GetFilesHandler : IRequestHandler<GetFilesRequest, IEnumerable<FileModel>>
{
  private readonly IFileRepository _fileRepository;
  private readonly IMapper _mapper;

  public GetFilesHandler ( IFileRepository fileRepository,
    IMapper mapper )
  {
    _fileRepository = fileRepository;
    _mapper = mapper;
  }
  public async Task<IEnumerable<FileModel>> Handle ( GetFilesRequest request, CancellationToken cancellationToken )
  {
    return (await _fileRepository.GetFilesAsync (cancellationToken))
      .Select (x => _mapper.Map<UploadedFile, FileModel> (x));
  }
}
