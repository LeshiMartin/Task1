using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class UploadedFile:BaseEntity
{
  public string FileName { get; set; } = string.Empty;
  public string FileUri { get; set; } =string.Empty;
  public ICollection<FileRow> FileRows { get; set; } = new HashSet<FileRow>();
}