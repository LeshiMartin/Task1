namespace Core.Entities;
public class BaseEntity
{
  public int Id { get; set; }
  public DateTime InsertTime { get;init; }   =DateTime.Now;
}
