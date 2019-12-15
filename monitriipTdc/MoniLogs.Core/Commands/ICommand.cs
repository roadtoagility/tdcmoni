namespace MoniLogs.Core.Commands
{
    public interface ICommand<T>
    {
        void Execute(T parameter);
    }
}