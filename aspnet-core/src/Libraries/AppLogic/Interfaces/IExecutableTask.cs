namespace AppLogic.Interfaces;

public interface IExecutableTask
{
  Task Execute ( CancellationToken cancellationToken );
}