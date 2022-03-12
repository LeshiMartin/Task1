using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo ("DALTests")]
namespace DAL;
internal class FileRepository : IFileRepository
{
  private readonly CoreDbContext _dbContext;

  public FileRepository ( IDbContextFactory<CoreDbContext> contextFactory )
  {
    _dbContext = contextFactory.CreateDbContext ();
  }
  public async Task<IEnumerable<FileRow>> GetLastUploadRowsAsync ( CancellationToken cancellationToken )
  {
    var lastUploadedId = await GetLastUploadedIdAsync (cancellationToken);
    if ( lastUploadedId == 0 )
      return Array.Empty<FileRow> ();

    return await _dbContext
      .FileRows!
      .Where (x => x.UploadedFileId == lastUploadedId)
      .ToArrayAsync (cancellationToken);
  }

  public Task<int> UpdateFileAsync(UploadedFile file, CancellationToken cancellationToken)
  {
    if (file == null) throw new ArgumentNullException(nameof(file));
    _dbContext.UploadedFiles!.Update(file);
    return _dbContext.SaveChangesAsync(cancellationToken);
  }

  private async Task<int> GetLastUploadedIdAsync ( CancellationToken cancellationToken )
  {
    var lastUploadedId = await _dbContext.UploadedFiles!
      .Select (x => x.Id)
      .OrderByDescending (x => x)
      .FirstOrDefaultAsync (cancellationToken);
    return lastUploadedId;
  }

  public async Task<IEnumerable<UploadedFile>> GetUploadedFileAsync ( CancellationToken cancellationToken )
  {
    return await _dbContext.UploadedFiles!.ToArrayAsync (cancellationToken);
  }

  public async Task<int> InsertUploadedFileAsync ( UploadedFile file, CancellationToken cancellationToken )
  {
    if ( file == null )
      throw new ArgumentNullException (nameof (file));
    await _dbContext.UploadedFiles!.AddAsync (file, cancellationToken);
    await _dbContext.SaveChangesAsync (cancellationToken);
    return file.Id;
  }

  public async Task<int> InsertFileRowsAsync ( IEnumerable<FileRow> rows, CancellationToken cancellationToken )
  {
    if ( rows == null )
      throw new ArgumentNullException (nameof (rows));
    await _dbContext.AddRangeAsync (rows, cancellationToken);
    return await _dbContext.SaveChangesAsync (cancellationToken);

  }
}
