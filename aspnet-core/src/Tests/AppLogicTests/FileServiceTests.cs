using AppLogic.FileServices;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppLogicTests;
public class FileServiceTests
{
  private readonly FileService _fileService;
  private readonly CancellationToken _cancellationToken;

  public FileServiceTests ()
  {
    _fileService = new FileService ();
    _cancellationToken = new CancellationTokenSource ().Token;
  }

  private IFormFile MockFormFile ()
  {
    var fileMock = Substitute.For<IFormFile> ();
    //Setup mock file using a memory stream
    var content = "Hello World from a Fake File";
    var fileName = "test.pdf";
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

  [Fact]
  public async Task WriteFileAsync_Should_Throw_Argument_Null_Exception_Parameter_Is_Null ()
  {
    await Assert
      .ThrowsAsync<ArgumentNullException> (() => _fileService.WriteFileAsync (default!, _cancellationToken));
  }

  [Fact]
  public async Task WriteFileAsync_Should_Return_The_Path_Of_The_Newly_Created_File ()
  {
    var response = await _fileService.WriteFileAsync (MockFormFile (), _cancellationToken);
    response.Should ().NotBeEmpty ();
  }

  [Fact]
  public async Task ReadFileAsync_Should_Throw_Argument_Null_Exception_Parameter_Is_Null ()
  {
    await Assert
      .ThrowsAsync<ArgumentNullException> (() => _fileService.ReadFileAsync (default!, _cancellationToken));
  }

  [Fact]
  public async Task ReadFileAsync_Should_Return_The_Content_Of_TheFile ()
  {
    var content = await _fileService.ReadFileAsync ("TestFile.txt", _cancellationToken);
    content.Should ().NotBeEmpty ();
  }

  [Fact]
  public async Task RemoveFileAsync_Should_Throw_Argument_Null_Exception_Parameter_Is_Null ()
  {
    await Assert
      .ThrowsAsync<ArgumentNullException> (() => _fileService.RemoveFileAsync (default!, _cancellationToken));
  }

  [Fact]
  public async Task RemoveFileAsync_Should_Return_One_After_Complete ()
  {
    var response = await _fileService.RemoveFileAsync ("File.txt", _cancellationToken);
    response.Should ().Be (1);
  }

  [Fact]
  public void T()
  {
    var content = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "TestFile.txt"));
    var arr = content.Split(new[] { '\r', '-', '\n' }, StringSplitOptions.RemoveEmptyEntries);
  }
}