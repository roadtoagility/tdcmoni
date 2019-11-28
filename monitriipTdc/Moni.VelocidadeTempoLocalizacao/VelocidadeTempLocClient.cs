using System.Threading;
using System.Threading.Tasks;
using MoniLogs.Core.Entities;
using MoniLogs.Core.Infrastructure;
using NetMQ;
using NetMQ.Sockets;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class VelocidadeTempLocClient : IVelocidadeTempoLocalizacaoClient
    {
        
        private readonly PublisherSocket _publisherSocket;
        private readonly IFinisherSubscription _finisherSubscription;
        
        static readonly object _object = new object();  
        
        public VelocidadeTempLocClient(PublisherSocket publisherSocket, IFinisherSubscription finisherSubscription)
        {
            _finisherSubscription = finisherSubscription;
            _publisherSocket = publisherSocket;
        }
        
        public string Send(Envelope envelope)
        {
            var onibus_request = Newtonsoft.Json.JsonConvert.SerializeObject(envelope);
            
            
            Monitor.Enter(_object);
                
            _finisherSubscription.Subscribe(envelope.Identity);
            _publisherSocket.SendMoreFrame("init").SendFrame(onibus_request);
                
            var finisher = _finisherSubscription.Receive(); //topic
            finisher = _finisherSubscription.Receive(); //
                
            System.Diagnostics.Debug.WriteLine("Received: " + finisher);
            var messageValidated = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(finisher);
            
            Monitor.Exit(_object);


            return envelope.Identity;
        }
        
        public async Task<string> SendAsync(Envelope envelope)
        {
            var onibus_request = Newtonsoft.Json.JsonConvert.SerializeObject(envelope);

            await Task.Run(() =>
            {
                Monitor.Enter(_object);
                
                _finisherSubscription.Subscribe(envelope.Identity);
                _publisherSocket.SendMoreFrame("init").SendFrame(onibus_request);
                
                Monitor.Exit(_object);
            });
            
            
//            await Task.Run(() =>
//            {
//                Monitor.Enter(_object);
//                System.Diagnostics.Debug.WriteLine("Esperando resposta na fila: " + envelope.Identity);
//                
//                var finisher = _finisherSubscription.Receive(); //topic
//                finisher = _finisherSubscription.Receive(); //
//                
//                System.Diagnostics.Debug.WriteLine("Received: " + finisher);
//                var messageValidated = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(finisher);
//                
//                Monitor.Exit(_object);
//                
//                erros = messageValidated.ValidationMessages;
//            });
            
            return await Task.FromResult(envelope.Identity);
        }
    }
}
