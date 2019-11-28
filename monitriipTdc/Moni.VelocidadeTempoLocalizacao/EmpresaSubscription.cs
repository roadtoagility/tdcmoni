using MoniLogs.Core.Infrastructure;
using NetMQ;
using NetMQ.Sockets;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class EmpresaSubscription : IEmpresaSubscription
    {
        private string _topic;

        private readonly SubscriberSocket _subscriberSocket;

        public EmpresaSubscription(SubscriberSocket subscriberSocket)
        {
            _subscriberSocket = subscriberSocket;
        }
        
        public void Subscribe(string topic)
        {
            _subscriberSocket.Subscribe(topic);
            _topic = topic;
        }

        public string Receive()
        {
            return _subscriberSocket.ReceiveFrameString();
        }
    } 
}