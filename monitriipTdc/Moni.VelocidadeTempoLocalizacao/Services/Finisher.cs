using System;
using System.Collections.Generic;
using MoniLogs.Core;
using MoniLogs.Core.Entities;
using MoniLogs.Core.Infrastructure;
using NetMQ;
using NetMQ.Sockets;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class ValidatedMessage
    {
        public ValidatedMessage(Envelope envelope)
        {
            Message = envelope;
            Count = 0;
        }
        
        public int Count { get; set; }
        public Envelope Message { get; set; }
    }
    
    public class Finisher : IService
    {
        private Dictionary<string, ValidatedMessage> _validatedMessages;
        private ICacheGateway _cache;
        private readonly string  PUBLISHER;
        private readonly string ROUTER;
        
        
        public Finisher(string ipRouter, string ipPublisher)
        {
            PUBLISHER = $"tcp://{ipPublisher}:15000";
            ROUTER = $"tcp://{ipRouter}:14000";
            _validatedMessages = new Dictionary<string, ValidatedMessage>();
            _cache = new CacheGateway();
        }
        
        public void Execute()
        {
            using (var reporterFinisher = new PublisherSocket(PUBLISHER))
            using (var jobFinisher = new RouterSocket(ROUTER)) 
            {
                Console.WriteLine("Finisher ONNNNNNNNNNNNNNNNNN");
                Console.WriteLine($"Listening running on: {ROUTER}");
                
                while (true)
                {
                    try
                    {
                        var identity = jobFinisher.ReceiveFrameString();
                        Console.WriteLine("identity: " + identity);
                        var message = jobFinisher.ReceiveFrameString();
                        Console.WriteLine("message: "+ message);
                        
                        var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(message);

                        if (_validatedMessages.ContainsKey(envelope.Identity))
                        {
                            var validatedMessage = _validatedMessages[envelope.Identity];
                            Console.WriteLine("Localizado " + envelope.Identity);
                            
                            validatedMessage.Count++;
                            validatedMessage.Message.ValidationMessages.AddRange(envelope.ValidationMessages);

                            if (validatedMessage.Count == 3)
                            {
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(validatedMessage.Message);
                                reporterFinisher.SendMoreFrame(envelope.Identity).SendFrame(json);
                                Console.WriteLine("Finisher sending " + envelope.Identity);

                                _cache.SetValue(envelope.Identity, json);
                                
                                _validatedMessages.Remove(envelope.Identity);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Criado " + envelope.Identity);
                            var validated = new ValidatedMessage(envelope);
                            validated.Count++;
                            _validatedMessages.Add(envelope.Identity, validated);
                        }
                    }
                    catch(Exception ex) //Drop message
                    {
                        Console.WriteLine("Erro na validação do dado: " + ex.Message);
                    }
                }
            }
        }
    }
}