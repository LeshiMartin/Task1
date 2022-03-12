using Core.Entities;

namespace Core;

public interface IFileRepository
{

  Task<IEnumerable<FileRow>> GetLastUploadRowsAsync ( CancellationToken cancellationToken );
  Task<int> UpdateFileAsync ( UploadedFile file, CancellationToken cancellationToken );
  Task<IEnumerable<UploadedFile>> GetUploadedFileAsync ( CancellationToken cancellationToken );
  Task<int> InsertUploadedFileAsync ( UploadedFile file, CancellationToken cancellationToken );
  Task<int> InsertFileRowsAsync ( IEnumerable<FileRow> rows, CancellationToken cancellationToken );
}