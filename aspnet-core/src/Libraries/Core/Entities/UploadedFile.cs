namespace Core.Entities;
public class UploadedFile : BaseEntity
{
  public string FileName { get; set; } = string.Empty;
  public string FileUri { get; set; } = string.Empty;
  public ICollection<FileRow> FileRows { get; set; } = new HashSet<FileRow> ();
  public int FileStatus { get; set; } = (int) UploadFileStatus.NotProcessed;
}

public enum UploadFileStatus
{
  NotProcessed = 10,
  InProcess = 20,
  Processed = 30,
  InValid = 40
}