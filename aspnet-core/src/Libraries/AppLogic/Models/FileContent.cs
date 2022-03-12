namespace AppLogic.Models;
internal class FileContent
{
  public FileContent(string content)
  {
    Content = content;
  }
  public string Content { get;private set; } 

  public string[][] ProcessContent()
  {
    return Content.Split(new[] { '\r', '-', '\n'}, StringSplitOptions.RemoveEmptyEntries ).Select(x => x.Split(',')).ToArray();
  }
}
