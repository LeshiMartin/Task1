namespace Core.Entities;

public class FileRow : BaseEntity
{
  public string Color { get; private set; } = string.Empty;
  public string Label { get; private set; } = string.Empty;
  public int Number { get; private set; }

  public int UploadedFileId { get; private set; }
  public UploadedFile? UploadedFile { get; set; }
}