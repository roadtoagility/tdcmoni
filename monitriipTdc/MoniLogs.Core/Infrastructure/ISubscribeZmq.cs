namespace MoniLogs.Core.Infrastructure
{
    public interface ISubscribeZmq
    {
        void Subscribe(string topic);
        string Receive();
    }
}