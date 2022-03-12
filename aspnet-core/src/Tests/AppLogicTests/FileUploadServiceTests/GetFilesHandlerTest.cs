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

public class GetFilesHandlerTest
{
  private readonly GetFilesHandler _handler;

  public GetFilesHandlerTest ()
  {
    var repo = Substitute.For<IFileRepository> ();
    var mapper = Substitute.For<IMapper> ();
    repo.GetFilesAsync (Arg.Any<CancellationToken> ())
      .Returns (x => new List<UploadedFile> () { new () { FileName = "test.txt", FileUri = "Test.txt" } });
    mapper.Map<UploadedFile, FileModel> (Arg.Any<UploadedFile> ()).Returns (new FileModel ()
    {
      FileName = "test.txt"
    });

    _handler = new GetFilesHandler (repo, mapper);
  }


  [Fact]
  public async Task GetFilesAsync_Should_Return_Collection_Of_FileModel ()
  {
    var response = await _handler.Handle (new GetFilesRequest (), new CancellationToken ());
    var fileModels = response as FileModel[] ?? response.ToArray ();
    fileModels.Should ().NotBeNull ();
    fileModels.Should ().NotBeEmpty ();
  }
}