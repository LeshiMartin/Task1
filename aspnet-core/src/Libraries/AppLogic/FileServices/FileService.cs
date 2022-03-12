using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
[assembly: InternalsVisibleTo ("AppLogicTests")]
namespace AppLogic.FileServices;
internal class FileService : IFileService
{
  public async Task<string> WriteFileAsync ( IFormFile file, CancellationToken cancellationToken )
  {
    if ( file == null )
      throw new ArgumentNullException (nameof (file));
    var fileNameConstructed =
      $"{Path.GetFileNameWithoutExtension (file.FileName)}_{Guid.NewGuid ().ToString ()[ ..6 ]}.txt";
    var relativePath = Path.Combine ("FileInputs", fileNameConstructed);
    var absPath = Path.Combine (Directory.GetCurrentDirectory (), relativePath);
    await using var fs = new FileStream (absPath, FileMode.Create, FileAccess.Write);
    await file.CopyToAsync (fs, cancellationToken);
    return relativePath;

  }

  public Task<string> ReadFileAsync ( string uri, CancellationToken cancellationToken )
  {
    if ( uri == null )
      throw new ArgumentNullException (nameof (uri));
    var absPath = Path.Combine (Directory.GetCurrentDirectory (), uri);
    return File.Exists(absPath)? 
      File.ReadAllTextAsync (absPath, cancellationToken):
      Task.FromResult(string.Empty);
  }

  public Task<int> RemoveFileAsync ( string uri, CancellationToken cancellationToken )
  {
    if ( uri == null )
      throw new ArgumentNullException (nameof (uri));
    var absPath = Path.Combine (Directory.GetCurrentDirectory (), uri);
    if ( File.Exists (absPath) )
      File.Delete (absPath);
    return Task.FromResult (1);
  }
}
