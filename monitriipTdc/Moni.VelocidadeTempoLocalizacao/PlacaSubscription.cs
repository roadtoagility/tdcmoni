using MoniLogs.Core;
using MoniLogs.Core.Infrastructure;
using NetMQ;
using NetMQ.Sockets;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class PlacaSubscription : IPlacaSubscription
    {
        private readonly SubscriberSocket _subscriberSocket;

        public PlacaSubscription(SubscriberSocket subscriberSocket)
        {
            _subscriberSocket = subscriberSocket;
        }
        
        private string _topic;
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