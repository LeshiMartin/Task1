namespace AppLogic.Interfaces;

public interface INotifyFileIsProcessed
{
  Task FileIsProcessed ( int id );
}