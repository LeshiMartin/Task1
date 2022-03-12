using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppLogic.FileUploadService;
using AppLogic.Models;
using AutoMapper;
using Core;
using Core.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppLogicTests.FileUploadServiceTests;

public class GetLastUploadedRowsHandlerTest
{
  private readonly GetLastUploadedRowsHandler _handler;

  public GetLastUploadedRowsHandlerTest ()
  {
    var repo = Substitute.For<IFileRepository> ();
    var mapper = Substitute.For<IMapper> ();
    repo.GetLastUploadRowsAsync (Arg.Any<CancellationToken> ())
      .Returns (x => new List<FileRow> () { new FileRow ("cl", "dsd", 2) });
    mapper.Map<FileRow, FileRowModel> (Arg.Any<FileRow> ()).Returns (new FileRowModel ()
    {
      Color = "cl",
      Label = "dsd",
      Value = 2
    });

    _handler = new GetLastUploadedRowsHandler (repo, mapper);
  }

  [Fact]
  public async Task Handle_Should_Return_Collection_Of_FileRowModel ()
  {
    var token = new CancellationToken ();
    var response = await _handler.Handle (new GetLastUploadedRowsRequest (), token);
    var fileRowModels = response as FileRowModel[] ?? response.ToArray ();
    fileRowModels.Should ().NotBeNull ();
    fileRowModels.Should ().NotBeEmpty ();
  }

}