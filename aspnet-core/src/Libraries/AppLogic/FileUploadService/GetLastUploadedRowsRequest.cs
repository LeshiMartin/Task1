using AppLogic.Models;
using AutoMapper;
using Core;
using Core.Entities;
using MediatR;

namespace AppLogic.FileUploadService;

public record GetLastUploadedRowsRequest () : IRequest<IEnumerable<FileRowModel>>;


internal class GetLastUploadedRowsHandler : IRequestHandler<GetLastUploadedRowsRequest, IEnumerable<FileRowModel>>
{
  private readonly IFileRepository _fileRepository;
  private readonly IMapper _mapper;

  public GetLastUploadedRowsHandler ( IFileRepository fileRepository,
    IMapper mapper )
  {
    _fileRepository = fileRepository;
    _mapper = mapper;
  }
  public async Task<IEnumerable<FileRowModel>> Handle ( GetLastUploadedRowsRequest request, CancellationToken cancellationToken )
  {
    return (await _fileRepository
      .GetLastUploadRowsAsync (cancellationToken))
      .Select (x => _mapper.Map<FileRow, FileRowModel> (x));
  }
}