namespace AppLogic.Models;
public class FileModel
{
  public int Id { get; set; }
  public string FileName { get; set; } = string.Empty;
  public int FileStatusValue { get; set; }
  public string FileStatus { get; set; } = string.Empty;
  public DateTime DateOfUpload { get; set; }
}
