using Core.Entities;

namespace Core;

public interface IFileRepository
{

  Task<IEnumerable<FileRow>> GetLastUploadRowsAsync ( CancellationToken cancellationToken );
  Task<int> UpdateFileAsync ( UploadedFile file, CancellationToken cancellationToken );
  Task<IEnumerable<UploadedFile>> GetFilesAsync ( CancellationToken cancellationToken );
  Task<int> InsertFileAsync ( UploadedFile file, CancellationToken cancellationToken );
  Task<int> InsertFileRowsAsync ( IEnumerable<FileRow> rows, CancellationToken cancellationToken );
  Task<UploadedFile?> GetFirstNotProcessedFile(CancellationToken  cancellationToken);
}