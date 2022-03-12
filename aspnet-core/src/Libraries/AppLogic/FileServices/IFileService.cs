using Microsoft.AspNetCore.Http;

namespace AppLogic.FileServices;

public interface IFileService
{
  Task<string> WriteFileAsync ( IFormFile file, CancellationToken cancellationToken );
  Task<string> ReadFileAsync ( string uri, CancellationToken cancellationToken );
  Task<int> RemoveFileAsync ( string uri, CancellationToken cancellationToken );
}