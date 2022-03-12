using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using DAL;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace DALTests;
public class FileRepositoryTests
{
  private readonly CoreDbContext _dbContext;
  private readonly FileRepository _fileRepo;
  private readonly CancellationToken _cancellationToken;

  public FileRepositoryTests ()
  {
    var contextFactory = Substitute.For<IDbContextFactory<CoreDbContext>> ();
    _dbContext = DbContextProvider.GetDbContext ();
    contextFactory.CreateDbContext ().Returns (x => _dbContext);
    _fileRepo = new FileRepository (contextFactory);
    _cancellationToken = new CancellationTokenSource ().Token;
  }
  [Fact]
  public async Task GetLastUploadRowsAsync_Should_Return_EmptyCollection ()
  {
    var response = await _fileRepo.GetLastUploadRowsAsync (_cancellationToken);
    response.Should ().NotBeNull ();
    var responseArr = response.ToArray ();
    responseArr.Should ().BeEmpty ();
  }

  [Fact]
  public async Task GetLastUploadRowsAsync_Should_Return_FileRows ()
  {
    var mockFiles = MockFiles ();
    await _dbContext.UploadedFiles!.AddRangeAsync (mockFiles, _cancellationToken);
    await _dbContext.SaveChangesAsync (_cancellationToken);

    var r = await _fileRepo.GetLastUploadRowsAsync (_cancellationToken);
    var rArr = r.ToArray ();
    rArr.Should ().NotBeEmpty ();
    rArr.All (x => x.UploadedFileId == 2).Should ().BeTrue ();

  }

  [Fact]
  public async Task GetUploadedFilesAsync_Should_Return_Collection_Of_UploadedFiles ()
  {
    var mockFiles = MockFiles ();
    await _dbContext.AddRangeAsync (mockFiles, _cancellationToken);
    await _dbContext.SaveChangesAsync (_cancellationToken);

    var response = await _fileRepo.GetFilesAsync (_cancellationToken);
    response.Should ().NotBeNull ();
  }

  [Fact]
  public async Task InsertUploadedFileAsync_Should_Throw_ArgumentNullException_When_Parameter_Is_Null ()
  {
    await Assert.ThrowsAnyAsync<ArgumentNullException> (
      () => _fileRepo.InsertFileAsync (default!, _cancellationToken));
  }

  [Fact]
  public async Task InsertUploadedFileAsync_Should_Insert_New_Entry_In_The_Db ()
  {
    var file = new UploadedFile ()
    {
      FileName = "TestName_Test",
      FileUri = "TestName_Test.txt"
    };

    var response = await _fileRepo.InsertFileAsync (file, _cancellationToken);
    response.Should ().BeGreaterThan (0);
  }

  [Fact]
  public async Task InsertFileRowsAsync_Should_Throw_ArgumentNullException_When_Parameter_Is_Null ()
  {
    await Assert.ThrowsAnyAsync<ArgumentNullException> (
      () => _fileRepo.InsertFileRowsAsync (default!, _cancellationToken));
  }

  [Fact]
  public async Task InsertFileRowsAsync_Should_Insert_New_Entry_In_The_Db ()
  {
    var file = new UploadedFile ()
    {
      FileName = "TestName_Test",
      FileUri = "TestName_Test.txt"
    };
    await _dbContext.AddAsync (file, _cancellationToken);
    await _dbContext.SaveChangesAsync (_cancellationToken);
    var row = new FileRow[] { new ("blue", "label", 34) { UploadedFileId = file.Id } };
    var response = await _fileRepo.InsertFileRowsAsync (row, _cancellationToken);
    response.Should ().BeGreaterThan (0);
    var dbRows = _dbContext.FileRows!.Where (x => x.UploadedFileId == file.Id);
    dbRows.Should ().HaveCount (1);

  }
  [Fact]
  public async Task UpdateFileAsync_Should_Throw_ArgumentNullException_When_Parameter_Is_Null ()
  {
    await Assert.ThrowsAsync<ArgumentNullException> (() => _fileRepo.UpdateFileAsync (default!, _cancellationToken));
  }

  [Fact]
  public async Task UpdateFileAsync_Should_Update_The_File ()
  {
    var file = new UploadedFile ()
    {
      FileName = "TestName_Test",
      FileUri = "TestName_Test.txt"
    };
    await _dbContext.AddAsync (file, _cancellationToken);
    await _dbContext.SaveChangesAsync (_cancellationToken);

    file.FileStatus = (int)UploadFileStatus.InProcess;
    await _fileRepo.UpdateFileAsync(file, _cancellationToken);
    var fUpdated = await _dbContext.UploadedFiles!.SingleAsync(x => x.Id == file.Id, _cancellationToken);
    fUpdated.FileStatus.Should().Be((int)UploadFileStatus.InProcess);
  }


  private static HashSet<UploadedFile> MockFiles ()
  {
    return new HashSet<UploadedFile> ()
    {
      new()
      {
        Id = 1,
        FileName = "TestName",
        FileUri = "TestName.txt",
        
        FileRows = new List<FileRow>()
        {
          new("red", "label", 2)
          {
            UploadedFileId = 1
          }
        }
      },
      new()
      {
        Id = 2,
        FileName = "TestName2",
        FileUri = "TestName2.txt",
        FileStatus = (int)UploadFileStatus.Processed,
        FileRows = new List<FileRow>()
        {
          new("blue", "label", 3)
          {
            UploadedFileId = 2
          }
        }
      }
    };
  }


  ~FileRepositoryTests ()
  {
    _dbContext.Dispose ();
  }
}