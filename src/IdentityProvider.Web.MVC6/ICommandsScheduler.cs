using System.Threading.Tasks;

public interface ICommandsScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}