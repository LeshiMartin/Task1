namespace Core.Entities;

public class FileRow : BaseEntity
{
  private FileRow()
  {
    
  }

  public FileRow(string color,string label,int number)
  {
    Color = color;
    Label = label;
    Number = number;
  }

  public string Color { get; private set; } = string.Empty;
  public string Label { get; private set; } = string.Empty;
  public int Number { get; private set; }

  public int UploadedFileId { get; init; }
  public UploadedFile? UploadedFile { get; set; }
}