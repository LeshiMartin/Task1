namespace AppLogic.Interfaces;

public interface INotifyFileInProcess
{
  Task FileIsInProcess(int id);
}