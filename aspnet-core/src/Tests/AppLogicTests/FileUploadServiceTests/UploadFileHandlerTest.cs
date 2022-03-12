using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AppLogic;
using AppLogic.FileServices;
using AppLogic.FileUploadService;
using AutoMapper;
using Core;
using Core.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace AppLogicTests.FileUploadServiceTests;

public class UploadFileHandlerTest
{
  private readonly UploadFileHandler _handler;

  public UploadFileHandlerTest ()
  {
    var mapConfig = new MapperConfiguration (x =>
    {
      x.AddProfiles (new List<Profile> { new MappingProfile () });
    });
    var mapper = mapConfig.CreateMapper ();
    var fileRepo = Substitute.For<IFileRepository> ();
    fileRepo.InsertFileAsync (Arg.Any<UploadedFile> (), Arg.Any<CancellationToken> ()).Returns (1);
    var fileService = Substitute.For<IFileService> ();
    fileService.WriteFileAsync (Arg.Any<IFormFile> (), Arg.Any<CancellationToken> ()).Returns ("path");
    _handler = new UploadFileHandler (fileRepo, mapper, fileService);
  }

  [Fact]
  public async Task Handle_Should_Throw_Argument_NullException_When_Parameter_Is_Null ()
  {
    await Assert.ThrowsAsync<ArgumentNullException> (() =>
       _handler.Handle (new UploadFileRequest (default!), new CancellationToken ()));
  }

  [Fact]
  public async Task Handle_Should_Throw_ArgumentException_When_Parameter_Is_Not_TxtFile ()
  {
    await Assert.ThrowsAsync<ArgumentException> (() =>
       _handler.Handle (new UploadFileRequest (MockFormFile ("T.pdf")), new CancellationToken ()));
  }

  [Fact]
  public async Task Handle_Should_Return_Instance_Of_FileModel_With_Status_NotProcessed_OnSuccessfull_Insert ()
  {
    var response = await _handler.Handle (new UploadFileRequest (MockFormFile ("t.txt")), new CancellationToken ());
    response.Should ().NotBeNull ();
    response.FileStatusValue.Should ().Be ((int) UploadFileStatus.NotProcessed);
  }

  private IFormFile MockFormFile ( string fileName )
  {
    var fileMock = Substitute.For<IFormFile> ();
    //Setup mock file using a memory stream
    var content = "Hello World from a Fake File";
    var ms = new MemoryStream ();
    var writer = new StreamWriter (ms);
    writer.Write (content);
    writer.Flush ();
    ms.Position = 0;
    fileMock.OpenReadStream ().Returns (ms);
    fileMock.FileName.Returns (fileName);
    fileMock.Length.Returns (ms.Length);
    return fileMock;
  }
}