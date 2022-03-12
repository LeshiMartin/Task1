namespace AppLogic.Models;

public class FileRowModel
{
  public int Id { get; set; }
  public string Label { get; set; } = string.Empty;
  public string Color { get; set; }=string.Empty;
  public int Value { get; set; }
  public DateTime InsertTime { get; set; }
}